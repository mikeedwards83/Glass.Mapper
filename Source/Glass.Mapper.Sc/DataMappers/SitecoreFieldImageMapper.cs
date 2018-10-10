using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldImageMapper
    /// </summary>
    public class SitecoreFieldImageMapper : AbstractSitecoreFieldMapper
    {
        private IMediaUrlOptionsResolver _mediaUrlOptionsResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldImageMapper"/> class.
        /// </summary>
        public SitecoreFieldImageMapper(): this (new MediaUrlOptionsResolver())
        {
        }
        public SitecoreFieldImageMapper(IMediaUrlOptionsResolver mediaUrlOptionsResolver)
            :  base(typeof(Image))
        {
            _mediaUrlOptionsResolver = mediaUrlOptionsResolver;
        }


        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            if (field.Value.IsNullOrEmpty())
            {
                return null;
            }

            Image img = new Image();
            ImageField scImg = new ImageField(field);
            var mediaUrlOptions = _mediaUrlOptionsResolver.GetMediaUrlOptions(config.MediaUrlOptions);
            MapToImage(img, scImg, mediaUrlOptions);

            return img;
        }

        public static void MapToImage(Image img, ImageField field, MediaUrlOptions mediaUrlOptions)
        {
            int height = 0;
            int.TryParse(field.Height, out height);
            int width = 0;
            int.TryParse(field.Width, out width);
            int hSpace = 0;
            int.TryParse(field.HSpace, out hSpace);
            int vSpace = 0;
            int.TryParse(field.VSpace, out vSpace);

            img.Alt = field.Alt;
            img.Border = field.Border;
            img.Class = field.Class;
            img.Height = height;
            img.HSpace = hSpace;
            img.MediaId = field.MediaID.Guid;
            img.MediaExists = field.MediaItem != null;

            if (field.MediaItem != null)
            {
                img.Src = MediaManager.GetMediaUrl(field.MediaItem, mediaUrlOptions);
                var fieldTitle = field.MediaItem.Fields["Title"];
                if (fieldTitle != null)
                    img.Title = fieldTitle.Value;
            }

            img.VSpace = vSpace;
            img.Width = width;
            img.Language = field.MediaLanguage;
        }

        public static void MapToImage(Image img, MediaItem imageItem)
        {
            /* int height = 0;
             int.TryParse(imageItem..Height, out height);
             int width = 0;
             int.TryParse(imageItem.Width, out width);
             int hSpace = 0;
             int.TryParse(imageItem.HSpace, out hSpace);
             int vSpace = 0;
             int.TryParse(imageItem.VSpace, out vSpace);*/

            img.Alt = imageItem.Alt;
            img.Title = imageItem.Title;
            // img.Border = imageItem.Border;
            // img.Class = imageItem.Class;
            // img.Height = height;
            // img.HSpace = hSpace;
            img.MediaId = imageItem.ID.Guid;
            img.Src = MediaManager.GetMediaUrl(imageItem);
            // img.VSpace = vSpace;
            // img.Width = width;
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <exception cref="Glass.Mapper.MapperException">No item with ID {0}. Can not update Media Item field.Formatted(newId)</exception>
        public override void SetField(Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Image img = value as Image;
            var item = field.Item;

            if (field == null) return;

            ImageField scImg = new ImageField(field);

            MapToField(scImg, img, item);
        }

        public static void MapToField(ImageField field, Image image, Item item)
        {
            if (image == null)
            {
                field.Clear();
                return;
            }

            if (field.MediaID.Guid != image.MediaId)
            {
                //this only handles empty guids, but do we need to remove the link before adding a new one?
                if (image.MediaId == Guid.Empty)
                {
                    ItemLink link = new ItemLink(item.Database.Name, item.ID, field.InnerField.ID, field.MediaItem.Database.Name, field.MediaID, field.MediaItem.Paths.Path);
                    field.RemoveLink(link);
                }
                else
                {
                    ID newId = new ID(image.MediaId);
                    Item target = item.Database.GetItem(newId);
                    if (target != null)
                    {
                        field.MediaID = newId;
                        ItemLink link = new ItemLink(item.Database.Name, item.ID, field.InnerField.ID, target.Database.Name, target.ID, target.Paths.FullPath);
                        field.UpdateLink(link);

                    }
                    else throw new MapperException("No item with ID {0}. Can not update Media Item field".Formatted(newId));
                }
            }

            if (image.Height > 0)
                field.Height = image.Height.ToString();
            if (image.Width > 0)
                field.Width = image.Width.ToString();
            if (image.HSpace > 0)
                field.HSpace = image.HSpace.ToString();
            if (image.VSpace > 0)
                field.VSpace = image.VSpace.ToString();

            if (field.Alt.HasValue() || image.Alt.HasValue())
                field.Alt = image.Alt ?? string.Empty;
            if (field.Border.HasValue() || image.Border.HasValue())
                field.Border = image.Border ?? string.Empty;
            if (field.Class.HasValue() || image.Class.HasValue())
                field.Class = image.Class ?? string.Empty;
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
            var item = context.Service.Database.GetItem(new ID(fieldValue));

            if (item == null)
            {
                return null;
            }

            var imageItem = new MediaItem(item);
            var image = new Image();
            MapToImage(image, imageItem);
            return image;
        }
    }
}




