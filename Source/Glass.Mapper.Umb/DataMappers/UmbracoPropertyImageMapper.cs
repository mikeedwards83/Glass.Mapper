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
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.PropertyTypes;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldIUmbracoPropertyImageMappermageMapper
    /// </summary>
    public class UmbracoPropertyImageMapper : AbstractUmbracoPropertyMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoPropertyImageMapper" /> class.
        /// </summary>
        public UmbracoPropertyImageMapper()
            : base(typeof(Image))
        {
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override object GetProperty(Umbraco.Core.Models.Property property, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            Image img = new Image();
            /*var mediaService = new MediaService(new RepositoryFactory());
            mediaService.GetById(property.Value)
            ImageFiseld scImg = new ImagseField(field);

            int height = 0;
            int.TryParse(scImg.Height, out height);
            int width = 0;
            int.TryParse(scImg.Width, out width);
            int hSpace = 0;
            int.TryParse(scImg.HSpace, out hSpace);
            int vSpace = 0;
            int.TryParse(scImg.VSpace, out vSpace);

            img.Alt = scImg.Alt;
            img.Border = scImg.Border;
            img.Class = scImg.Class;
            img.Height = height;
            img.HSpace = hSpace;
            img.MediaId = scImg.MediaID.Guid;
            if (scImg.MediaItem != null)
                img.Src = MediaManager.GetMediaUrl(scImg.MediaItem);
            img.VSpace = vSpace;
            img.Width = width;*/

            return img;
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public override void SetProperty(Umbraco.Core.Models.Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
           /* Image img = value as Image;
            var item = field.Item;

            if (field == null) return;

            ImageField scImg = new ImageField(field);

            if (img == null)
            {
                scImg.Clear();
                return;
            }

            if (scImg.MediaID.Guid != img.MediaId)
            {
                //this only handles empty guids, but do we need to remove the link before adding a new one?
                if (img.MediaId == Guid.Empty)
                {
                    ItemLink link = new ItemLink(item.Database.Name, item.ID, scImg.InnerField.ID, scImg.MediaItem.Database.Name, scImg.MediaID, scImg.MediaItem.Paths.Path);
                    scImg.RemoveLink(link);
                }
                else
                {
                    ID newId = new ID(img.MediaId);
                    Item target = item.Database.GetItem(newId);
                    if (target != null)
                    {
                        scImg.MediaID = newId;
                        ItemLink link = new ItemLink(item.Database.Name, item.ID, scImg.InnerField.ID, target.Database.Name, target.ID, target.Paths.FullPath);
                        scImg.UpdateLink(link);
                    }
                    else throw new MapperException("No item with ID {0}. Can not update Media Item field".Formatted(newId));
                }
            }

            scImg.Height = img.Height.ToString();
            scImg.Width = img.Width.ToString();
            scImg.HSpace = img.HSpace.ToString();
            scImg.VSpace = img.VSpace.ToString();
            scImg.Alt = img.Alt;
            scImg.Border = img.Border;
            scImg.Class = img.Class;*/
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
