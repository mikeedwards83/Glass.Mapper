using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Fields;
using Sitecore.Links;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLinkMapper : AbstractSitecoreFieldMapper
    {
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            if (field == null || field.Value.Trim().IsNullOrEmpty()) return null;



            Link link = new Link();
            LinkField linkField = new LinkField(field);

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
                    if (linkField.TargetItem == null) link.Url = string.Empty;
                    else link.Url = LinkManager.GetItemUrl(linkField.TargetItem);
                    link.Type = LinkType.Internal;
                    link.TargetId = linkField.TargetID.Guid;

                    break;
                default:
                    return null;
                    break;

            }


            link.Anchor = linkField.Anchor;
            link.Class = linkField.Class;
            link.Text = linkField.Text;
            link.Title = linkField.Title;
            link.Target = linkField.Target;
            link.Query = linkField.QueryString;

            return link;
        }
    }
}
