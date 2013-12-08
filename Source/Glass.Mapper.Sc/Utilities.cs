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
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class Utilities
    /// </summary>
    public class Utilities : Mapper.Utilities
    {
        /// <summary>
        /// Converts a NameValueCollection in to HTML attributes
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

        public static Item CreateFakeItem(Dictionary<Guid, string> fields, string name = "itemName")
        {
            return CreateFakeItem(fields, new ID(Guid.NewGuid()), new Database("master"), name);
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

        /// <summary>
        /// Creates the URL options.
        /// </summary>
        /// <param name="urlOptions">The URL options.</param>
        /// <returns>UrlOptions.</returns>
        public static UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions)
        {
            UrlOptions defaultUrl = UrlOptions.DefaultOptions;

            if (urlOptions == 0) return defaultUrl;

            var t = (urlOptions & SitecoreInfoUrlOptions.AddAspxExtension);

            Func<SitecoreInfoUrlOptions, bool> flagCheck =
                option => (urlOptions & option) == option;


            //check for any default overrides
            defaultUrl.AddAspxExtension = flagCheck(SitecoreInfoUrlOptions.AddAspxExtension) ? true : defaultUrl.AddAspxExtension;
            defaultUrl.AlwaysIncludeServerUrl = flagCheck(SitecoreInfoUrlOptions.AlwaysIncludeServerUrl) ? true : defaultUrl.AlwaysIncludeServerUrl;
            defaultUrl.EncodeNames = flagCheck(SitecoreInfoUrlOptions.EncodeNames) ? true : defaultUrl.EncodeNames;
            defaultUrl.ShortenUrls = flagCheck(SitecoreInfoUrlOptions.ShortenUrls) ? true : defaultUrl.ShortenUrls;
            defaultUrl.SiteResolving = flagCheck(SitecoreInfoUrlOptions.SiteResolving) ? true : defaultUrl.SiteResolving;
            defaultUrl.UseDisplayName =flagCheck(SitecoreInfoUrlOptions.UseUseDisplayName) ? true : defaultUrl.UseDisplayName;


            if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingAlways))
                defaultUrl.LanguageEmbedding = LanguageEmbedding.Always;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingAsNeeded))
                defaultUrl.LanguageEmbedding = LanguageEmbedding.AsNeeded;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageEmbeddingNever))
                defaultUrl.LanguageEmbedding = LanguageEmbedding.Never;

            if (flagCheck(SitecoreInfoUrlOptions.LanguageLocationFilePath))
                defaultUrl.LanguageLocation = LanguageLocation.FilePath;
            else if (flagCheck(SitecoreInfoUrlOptions.LanguageLocationQueryString))
                defaultUrl.LanguageLocation = LanguageLocation.QueryString;

            return defaultUrl;

        }

        /// <summary>
        /// Creates the type of the generic.
        /// </summary>
        /// <param name="type">The generic type to create e.g. List&lt;&gt;</param>
        /// <param name="arguments">The list of subtypes for the generic type, e.g string in List&lt;string&gt;</param>
        /// <param name="parameters">List of parameters to pass to the constructor.</param>
        /// <returns>System.Object.</returns>
        public static object CreateGenericType(Type type, Type[] arguments, params  object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            object obj;
            if (parameters != null && parameters.Count() > 0)
				obj = GetActivator(genericType, parameters.Select(p => p.GetType()))(parameters);
            else
				obj = GetActivator(genericType)();
            return obj;
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
            if(item == null)
                throw new NullReferenceException("Item is null");

            Field field;
            if (ID.IsNullOrEmpty(fieldId))
            {
                field = item.Fields[fieldName];
            }
            else// if (item.Fields.Contains(fieldId) || item.Template.GetField(fieldId) != null)
            {
                field = item.Fields[fieldId];
            }

            return field;
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
        public static Item GetLanguageItem(Item foundItem, Language language)
        {
            if (foundItem == null) return null;

            var item = foundItem.Database.GetItem(foundItem.ID, language);
            if (item.Versions.Count > 0)
                return item;
            else
                return null;
        }
        /// <summary>
        /// Gets the language items.
        /// </summary>
        /// <param name="foundItems">The found items.</param>
        /// <param name="language">The language.</param>
        /// <returns>IEnumerable{Item}.</returns>
        public static IEnumerable<Item> GetLanguageItems(IEnumerable<Item> foundItems, Language language)
        {
            if (foundItems == null) return Enumerable.Empty<Item>();

            return foundItems.Select(x => GetLanguageItem(x, language)).Where(x => x != null);
        }
    }
}




