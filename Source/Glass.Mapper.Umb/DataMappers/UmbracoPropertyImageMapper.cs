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
using Umbraco.Core.Models;
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
        public override object GetProperty(Property property, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            if (property == null || property.Value == null)
                return null;

            var mediaService = new MediaService(new RepositoryFactory());
            int id;

            if (!int.TryParse(property.Value.ToString(), out id))
                return null;

            var image = mediaService.GetById(id);

            if (image != null)
            {
                int width;
                int.TryParse(image.Properties["umbracoWidth"].Value.ToString(), out width);
                int height;
                int.TryParse(image.Properties["umbracoHeight"].Value.ToString(), out height);
                int bytes;
                int.TryParse(image.Properties["umbracoBytes"].Value.ToString(), out bytes);

                var img = new Image
                    {
                        Id = image.Id,
                        Alt = image.Name,
                        Src = image.Properties["umbracoFile"].Value.ToString(),
                        Width = width,
                        Height = height,
                        Extension = image.Properties["umbracoExtension"].Value.ToString(),
                        Size = bytes
                    };
                return img;
            }

            return null;
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public override void SetProperty(Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            throw new NotImplementedException();
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

