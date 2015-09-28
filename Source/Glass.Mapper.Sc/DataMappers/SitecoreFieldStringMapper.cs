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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldStringMapper
    /// </summary>
    public class SitecoreFieldStringMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldStringMapper"/> class.
        /// </summary>
        public SitecoreFieldStringMapper()
            : base(typeof(string))
        {
        }

        private const string _richTextKey = "rich text";


        private static ConcurrentDictionary<Guid, bool> isRichTextDictionary = new ConcurrentDictionary<Guid, bool>();

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (field == null)
                return string.Empty;

            if (config.Setting == SitecoreFieldSettings.RichTextRaw)
            {
                return field.Value;
            }

            if (config.Setting == SitecoreFieldSettings.ForceRenderField)
            {
                return RunPipeline(field);
            }

            Guid fieldGuid = field.ID.Guid;

            // shortest route - we know whether or not its rich text
            if (isRichTextDictionary.ContainsKey(fieldGuid))
            {
                return GetResult(field, isRichTextDictionary[fieldGuid]);
            }

            // we don't know - it might still be rich text
            bool isRichText = field.TypeKey == _richTextKey;
            isRichTextDictionary.TryAdd(fieldGuid, isRichText);

            // now we know it isn't rich text - return the raw result.
            return GetResult(field, isRichText);
        }

        protected virtual string RunPipeline(Field field)
        {
            RenderFieldArgs renderFieldArgs = new RenderFieldArgs
            {
                Item = field.Item,
                FieldName = field.Name,
                DisableWebEdit = true
            };

            CorePipeline.Run("renderField", renderFieldArgs);

            return renderFieldArgs.Result.FirstPart + renderFieldArgs.Result.LastPart;
        }

        protected virtual string GetResult(Field field, bool isRichText)
        {
            if (!isRichText)
            {
                return field.Value;
            }

            return RunPipeline(field);
        }


        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <exception cref="System.NotSupportedException">It is not possible to save data from a rich text field when the data isn't raw.
        ///                     + Set the SitecoreFieldAttribute setting property to SitecoreFieldSettings.RichTextRaw for property {0} on type {1}.Formatted(config.PropertyInfo.Name, config.PropertyInfo.ReflectedType.FullName)</exception>
        public override void SetField(Sitecore.Data.Fields.Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (field == null)
            {
                return;
            }

            if(config.Setting == SitecoreFieldSettings.ForceRenderField)
            {
                throw new NotSupportedException("It is not possible to save data from a field when the data isn't raw."
                     + "In the SitecoreFieldAttribute remove the SitecoreFieldSettings.ForceRenderField valuea for property {0} on type {1}".Formatted(config.PropertyInfo.Name, config.PropertyInfo.ReflectedType.FullName));

            }

            if (field.Type.StartsWith("Rich Text") && config.Setting != SitecoreFieldSettings.RichTextRaw )
            {
                throw new NotSupportedException("It is not possible to save data from a rich text field when the data isn't raw."
                    + "Set the SitecoreFieldAttribute setting property to SitecoreFieldSettings.RichTextRaw for property {0} on type {1}".Formatted(config.PropertyInfo.Name, config.PropertyInfo.ReflectedType.FullName));
            }

            field.Value = value != null ? value.ToString() : null;
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            //this will only be used by the SitecoreFieldIEnumerableMapper
            return value as string;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            //this will only be used by the SitecoreFieldIEnumerableMapper
            return fieldValue;
        }
    }
}




