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
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Common;
using Sitecore.Configuration;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Xml.Xsl;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class Utilities
    /// </summary>
    public class Utilities : Mapper.Utilities
    {


        /// <summary>
        /// Converts a NameValueCollection into HTML attributes
        /// </summary>
        /// <param name="attributes">A list of attributes to convert</param>
        /// <returns>System.String.</returns>
        public static string ConvertAttributes(NameValueCollection attributes)
        {
            if (attributes == null || attributes.Count == 0) return "";

            StringBuilder sb = new StringBuilder();
            foreach (var key in attributes.AllKeys)
            {
                sb.AppendFormat("{0}='{1}' ".Formatted(key, attributes[key] ?? ""));
            }

            return sb.ToString();
        }
        /// <summary>
        /// Converts a SafeDictionary into HTML attributes
        /// </summary>
        /// <param name="attributes">A list of attributes to convert</param>
        /// <returns>System.String.</returns>
        public static string ConvertAttributes(SafeDictionary<string> attributes, string quotationMark)
        {
            if (attributes == null || attributes.Count == 0) return ""; 

            StringBuilder sb = new StringBuilder();
            foreach (var pair in attributes)
            {
                sb.AppendFormat("{0}={2}{1}{2} ".Formatted(pair.Key, pair.Value ??"", quotationMark));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldId">The field id.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Field.</returns>
        public static Field GetField(Item item, ID fieldId, string fieldName = "")
        {
            if (item == null)
                throw new NullReferenceException("Item is null");

            Field field;
            if (ID.IsNullOrEmpty(fieldId))
            {
                field = item.Fields[fieldName];
            }
            else
            {
                field = item.Fields[fieldId];
            }

            return field;
        }



        public static Item CreateFakeItem(Dictionary<Guid, string> fields, string name = "itemName")
        {
            return CreateFakeItem(fields, new ID(Guid.NewGuid()), Factory.GetDatabase("master"), name);
        }

        public static Item CreateFakeItem(Dictionary<Guid, string> fields, ID templateId, Database database, string name = "ItemName")
        {
            var id = new ID(Guid.NewGuid());
            var language = Language.Current;
            var version = Sitecore.Data.Version.Latest;

            var itemDefinition = new ItemDefinition(id, name, templateId, ID.Null);
            var fieldList = new FieldList();

            if (fields != null)
            {
                foreach (var fieldId in fields.Keys)
                {
                    fieldList.Add(new ID(fieldId), fields[fieldId]);
                }
            }

            var itemData = new ItemData(itemDefinition, language, version, fieldList);
            var item = new Item(id, itemData, database);
            return item;
        }

        public static Size ResizeImage(int imageW, int imageH, float imageScale, int w, int h, int maxW, int maxH)
        {

            Size size = new Size(w, h);
            Size imageSize = new Size(imageW, imageH);
            Size maxSize = new Size(maxW, maxH);

            if (imageW == 0 || imageH == 0)
                return size;

            return new GlassImageRender().GetFinalImageSize(imageSize, imageScale, size, maxSize);

        }

        /// <summary>
        /// Constructs the query string.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.String.</returns>
        public static string ConstructQueryString(NameValueCollection parameters)
        {
            var sb = new StringBuilder();

            foreach (String name in parameters)
                sb.Append(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters[name]), "&"));

            if (sb.Length > 0)
                return sb.ToString(0, sb.Length - 1);

            return String.Empty;
        }

        /// <summary>
        /// Gets the generic outer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        public static Type GetGenericOuter(Type type)
        {
            return type.GetGenericTypeDefinition();
        }

        /// <summary>
        /// Gets the language item.
        /// </summary>
        /// <param name="foundItem">The found item.</param>
        /// <param name="language">The language.</param>
        /// <returns>Item.</returns>
        public static Item GetLanguageItem(Item foundItem, Language language, Config config)
        {
            if (foundItem == null) return null;

            var item = foundItem.Database.GetItem(foundItem.ID, language);

            if (item == null || (item.Versions.Count == 0 && Utilities.DoVersionCheck(config)))
            {
                return null;
            }

            return item;
        }

        public static bool DoVersionCheck(Config config)
        {
            if (config.DisableVersionCount)
            {
                return false;
            }

            if (config != null && config.ForceItemInPageEditor && GlassHtml.IsInEditingMode)
            {
                return false;
            }

            return Switcher<VersionCountState>.CurrentValue != VersionCountState.Disabled;

        }



        /// <summary>
        /// Gets the language items.
        /// </summary>
        /// <param name="foundItems">The found items.</param>
        /// <param name="language">The language.</param>
        /// <param name="config"></param>
        /// <returns>IEnumerable{Item}.</returns>
        public static IEnumerable<Item> GetLanguageItems(IEnumerable<Item> foundItems, Language language, Config config)
        {
            if (foundItems == null) return Enumerable.Empty<Item>();

            return foundItems.Select(x => GetLanguageItem(x, language, config)).Where(x => x != null);
        }

        public class GlassImageRender : ImageRenderer
        {

            public Size GetFinalImageSize(Size imageSize, float imageScale, Size size, Size  maxSize )
            {
                return base.GetFinalImageSize(base.GetInitialImageSize(imageSize, imageScale, size), size, maxSize);
            }
        }
    }
}




