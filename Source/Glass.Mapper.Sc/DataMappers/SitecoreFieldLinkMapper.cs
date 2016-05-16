/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldLinkMapper
    /// </summary>
    public class SitecoreFieldLinkMapper : AbstractSitecoreFieldMapper
    {
        private readonly IUrlOptionsResolver _urlOptionsResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldLinkMapper"/> class.
        /// </summary>
        public SitecoreFieldLinkMapper() : this(new UrlOptionsResolver())
        {
        }

        public SitecoreFieldLinkMapper(IUrlOptionsResolver urlOptionsResolver) : base(typeof(Link))
        {
            _urlOptionsResolver = urlOptionsResolver;
        }


        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            if (field == null || field.Value.Trim().IsNullOrEmpty()) return null;

            Link link = new Link();
            LinkField linkField = new LinkField(field);

            link.Anchor = linkField.Anchor;
            link.Class = linkField.Class;
            link.Text = linkField.Text;
            link.Title = linkField.Title;
            link.Target = linkField.Target;
            link.Query = linkField.QueryString;

            switch (linkField.LinkType)
            {
                case "anchor":
                    link.Url = linkField.Anchor;
                    link.Type = LinkType.Anchor;
                    break;
                case "external":
                    link.Url = linkField.Url;
                    link.Type = LinkType.External;
                    break;
                case "mailto":
                    link.Url = linkField.Url;
                    link.Type = LinkType.MailTo;
                    break;
                case "javascript":
                    link.Url = linkField.Url;
                    link.Type = LinkType.JavaScript;
                    break;
                case "media":
                    if (linkField.TargetItem == null)
                        link.Url = string.Empty;
                    else
                    {
                        global::Sitecore.Data.Items.MediaItem media =
                            new global::Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                        link.Url = global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(media);
                    }
                    link.Type = LinkType.Media;
                    link.TargetId = linkField.TargetID.Guid;
                    break;
                case "internal":
                    var urlOptions = _urlOptionsResolver.CreateUrlOptions(config.UrlOptions);
                    link.Url = linkField.TargetItem == null ? string.Empty : LinkManager.GetItemUrl(linkField.TargetItem, urlOptions);
                    link.Type = LinkType.Internal;
                    link.TargetId = linkField.TargetID.Guid;
                    link.Text =  linkField.Text.IsNullOrEmpty() ? (linkField.TargetItem == null ? string.Empty : linkField.TargetItem.DisplayName) : linkField.Text;
                    break;
                default:
                    return null;
            }


         

            return link;
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <exception cref="Glass.Mapper.MapperException">
        /// No item with ID {0}. Can not update Link linkField.Formatted(newId)
        /// or
        /// No item with ID {0}. Can not update Link linkField.Formatted(newId)
        /// </exception>
        public override void SetField(Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Link link = value as Link;
            

            if (field == null) return;

            var item = field.Item;

            LinkField linkField = new LinkField(field);
            if (link == null || link.Type == LinkType.NotSet)
            {
                linkField.Clear();
                return;
            }


            switch (link.Type)
            {
                case LinkType.Internal:
                    linkField.LinkType = "internal";
                    if (linkField.TargetID.Guid != link.TargetId)
                    {
                        if (link.TargetId == Guid.Empty)
                        {
                            ItemLink iLink = new ItemLink(item.Database.Name, item.ID, linkField.InnerField.ID, linkField.TargetItem.Database.Name, linkField.TargetID, linkField.TargetItem.Paths.FullPath);
                            linkField.RemoveLink(iLink);
                        }
                        else
                        {
                            ID newId = new ID(link.TargetId);
                            Item target = item.Database.GetItem(newId);
                            if (target != null)
                            {
                                linkField.TargetID = newId;
                                ItemLink nLink = new ItemLink(item.Database.Name, item.ID, linkField.InnerField.ID, target.Database.Name, target.ID, target.Paths.FullPath);
                                linkField.UpdateLink(nLink);
                                linkField.Url = LinkManager.GetItemUrl(target);
                            }
                            else throw new MapperException("No item with ID {0}. Can not update Link linkField".Formatted(newId));
                        }

                    }
                    break;
                case LinkType.Media:
                    linkField.LinkType = "media";
                    if (linkField.TargetID.Guid != link.TargetId)
                    {
                        if (link.TargetId == Guid.Empty)
                        {
                            ItemLink iLink = new ItemLink(item.Database.Name, item.ID, linkField.InnerField.ID, linkField.TargetItem.Database.Name, linkField.TargetID, linkField.TargetItem.Paths.FullPath);
                            linkField.RemoveLink(iLink);
                        }
                        else
                        {
                            ID newId = new ID(link.TargetId);
                            Item target = item.Database.GetItem(newId);

                            if (target != null)
                            {
                                global::Sitecore.Data.Items.MediaItem media = new global::Sitecore.Data.Items.MediaItem(target);

                                linkField.TargetID = newId;
                                ItemLink nLink = new ItemLink(item.Database.Name, item.ID, linkField.InnerField.ID, target.Database.Name, target.ID, target.Paths.FullPath);
                                linkField.UpdateLink(nLink);
                                linkField.Url = global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(media);
                            }
                            else throw new MapperException("No item with ID {0}. Can not update Link linkField".Formatted(newId));
                        }

                    }
                    break;
                case LinkType.External:
                    linkField.LinkType = "external";
                    linkField.Url = link.Url;
                    break;
                case LinkType.Anchor:
                    linkField.LinkType = "anchor";
                    linkField.Url = link.Anchor;
                    break;
                case LinkType.MailTo:
                    linkField.LinkType = "mailto";
                    linkField.Url = link.Url;
                    break;
                case LinkType.JavaScript:
                    linkField.LinkType = "javascript";
                    linkField.Url = link.Url;
                    break;


            }



            if (!link.Anchor.IsNullOrEmpty())
                linkField.Anchor = link.Anchor;
            if (!link.Class.IsNullOrEmpty())
                linkField.Class = link.Class;
            if (!link.Text.IsNullOrEmpty())
                linkField.Text = link.Text;
            if (!link.Title.IsNullOrEmpty())
                linkField.Title = link.Title;
            if (!link.Query.IsNullOrEmpty())
                linkField.QueryString = link.Query;
            if (!link.Target.IsNullOrEmpty())
                linkField.Target = link.Target;
        }
    }
}




