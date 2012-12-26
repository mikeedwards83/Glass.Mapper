using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;

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
            var itemField = base.GetField(item);

            if (itemField == null || itemField.Value.Trim().IsNullOrEmpty()) return null;



            Link link = new Link();
            LinkField field = new LinkField(itemField);

            switch (field.LinkType)
            {
                case "anchor":
                    link.Url = field.Anchor;
                    link.Type = LinkType.Anchor;
                    break;
                case "external":
                    link.Url = field.Url;
                    link.Type = LinkType.External;
                    break;
                case "mailto":
                    link.Url = field.Url;
                    link.Type = LinkType.MailTo;
                    break;
                case "javascript":
                    link.Url = field.Url;
                    link.Type = LinkType.JavaScript;
                    break;
                case "media":
                    if (field.TargetItem == null)
                        link.Url = string.Empty;
                    else
                    {
                        global::Sitecore.Data.Items.MediaItem media =
                            new global::Sitecore.Data.Items.MediaItem(field.TargetItem);
                        link.Url = global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(media);
                        link.Type = LinkType.Media;
                        link.TargetId = field.TargetID.Guid;
                    }
                    break;
                case "internal":
                    if (field.TargetItem == null) link.Url = string.Empty;
                    else link.Url = LinkManager.GetItemUrl(field.TargetItem);
                    link.Type = LinkType.Internal;
                    link.TargetId = field.TargetID.Guid;

                    break;
                default:
                    return null;
                    break;

            }


            link.Anchor = field.Anchor;
            link.Class = field.Class;
            link.Text = field.Text;
            link.Title = field.Title;
            link.Target = field.Target;
            link.Query = field.QueryString;

            return link;
        }
    }
}
