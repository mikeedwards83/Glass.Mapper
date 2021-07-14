using System;
using System.Collections.Concurrent;
using System.Web;
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


        private static ConcurrentDictionary<Guid, bool> _isInternalLinkFieldDictionary = new ConcurrentDictionary<Guid, bool>();

        public const string InternalLinkKey = "internal link";

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldLinkMapper"/> class.
        /// </summary>
        public SitecoreFieldLinkMapper() : this(new UrlOptionsResolver())
        {
        }

        public SitecoreFieldLinkMapper(IUrlOptionsResolver urlOptionsResolver) 
            : this(urlOptionsResolver, typeof(Link))
        {
        }

        public SitecoreFieldLinkMapper(IUrlOptionsResolver urlOptionsResolver, Type type) 
            : base(type)
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


        protected void MapToLinkModel(Link link, InternalLinkField field, SitecoreFieldConfiguration config, GetOptionsSc getOptions)
        {
            var urlOptions = _urlOptionsResolver.CreateUrlOptions(config.UrlOptions, getOptions);

            link.Url = field.TargetItem == null ? string.Empty : SitecoreVersionAbstractions.GetItemUrl(field.TargetItem, urlOptions);
            link.Type = LinkType.Internal;
            link.TargetId = field.TargetItem == null ? Guid.Empty : field.TargetItem.ID.Guid;
            link.Text = field.TargetItem == null ? string.Empty : field.TargetItem.DisplayName;

        }




        protected void MapToLinkModel(Link link, LinkField linkField, SitecoreFieldConfiguration config, GetOptionsSc getOptions)
        {
            link.Anchor = linkField.Anchor;
            link.Class = linkField.Class;
            link.Style = linkField.GetAttribute("style");
            link.Text = linkField.Text;
            link.Title = linkField.Title;
            link.Target = linkField.Target;
            link.Query = HttpUtility.UrlDecode(linkField.QueryString);

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
                    link.Class = linkField.GetAttribute("style");
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
                        link.Url = SitecoreVersionAbstractions.GetMediaUrl(media);
                    }
                    link.Type = LinkType.Media;
                    link.TargetId = linkField.TargetID.Guid;
                    break;
                case "internal":
                    var urlOptions = _urlOptionsResolver.CreateUrlOptions(config.UrlOptions, getOptions);
                    link.Url = linkField.TargetItem == null ? string.Empty : SitecoreVersionAbstractions.GetItemUrl(linkField.TargetItem, urlOptions);
                    link.Type = LinkType.Internal;
                    link.TargetId = linkField.TargetID.Guid;
                    link.Text = linkField.Text.IsNullOrEmpty() ? (linkField.TargetItem == null ? string.Empty : linkField.TargetItem.DisplayName) : linkField.Text;
                    break;
                default:
                    link = null;
                    break;
            }


        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config,
            SitecoreDataMappingContext context)
        {

            if (field == null || field.Value.Trim().IsNullOrEmpty()) return null;

            Guid fieldGuid = field.ID.Guid;

            // shortest route - we know whether or not its rich text
            var isInternalLink =
                _isInternalLinkFieldDictionary.GetOrAdd(fieldGuid, (id) => field.TypeKey == InternalLinkKey);


            Link link = new Link();
            if (isInternalLink)
            {
                InternalLinkField internalLinkField = new Sitecore.Data.Fields.InternalLinkField(field);
                MapToLinkModel(link, internalLinkField, config, context.Options as GetOptionsSc);
            }
            else
            {
                LinkField linkField = new LinkField(field);

                MapToLinkModel(link, linkField, config, context.Options as GetOptionsSc);
            }
         

            return link;
        }

        protected void MapToLinkField(Link link, LinkField linkField)
        {
            var item = linkField.InnerField.Item;

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
                                linkField.Url = SitecoreVersionAbstractions.GetItemUrl(target);
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
                                var mediaUrl = SitecoreVersionAbstractions.GetMediaUrl(media);
                                linkField.Url = mediaUrl;
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
                linkField.QueryString = HttpUtility.UrlEncode(link.Query);
            if (!link.Target.IsNullOrEmpty())
                linkField.Target = link.Target;
        }

        protected void MapToLinkField(Link link, InternalLinkField linkField, SitecoreFieldConfiguration config)
        {
            var item = linkField.InnerField.Item;

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
                    ItemLink nLink = new ItemLink(item.Database.Name, item.ID, linkField.InnerField.ID, target.Database.Name, target.ID, target.Paths.FullPath);
                    linkField.UpdateLink(nLink);
                }
                else throw new MapperException("No item with ID {0}. Can not update Link linkField".Formatted(newId));
            }
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


            Guid fieldGuid = field.ID.Guid;

            // shortest route - we know whether or not its rich text
            var isInternalLink =
                _isInternalLinkFieldDictionary.GetOrAdd(fieldGuid, (id) => field.TypeKey == InternalLinkKey);

            if (isInternalLink)
            {
                InternalLinkField internalLinkField = new Sitecore.Data.Fields.InternalLinkField(field);
                MapToLinkField(link, internalLinkField, config);
            }
            else
            {
                LinkField linkField = new LinkField(field);
                if (link == null || link.Type == LinkType.NotSet)
                {
                    linkField.Clear();
                    return;
                }

                MapToLinkField(link, linkField);
            }
        }
    }
}




