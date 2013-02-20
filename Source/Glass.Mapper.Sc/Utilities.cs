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
    public class Utilities : Glass.Mapper.Utilities
    {
        /// <summary>
        /// Converts a NameValueCollection in to HTML attributes
        /// </summary>
        /// <param name="attributes">A list of attributes to convert</param>
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

        public static UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions)
        {
            UrlOptions defaultUrl = UrlOptions.DefaultOptions;

            if (urlOptions == 0) return defaultUrl;

            var t = (urlOptions & SitecoreInfoUrlOptions.AddAspxExtension);

            Func<SitecoreInfoUrlOptions, bool> flagCheck =
                (SitecoreInfoUrlOptions option) => (urlOptions & option) == option;


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
        /// </summary>
        /// <param name="type">The generic type to create e.g. List&lt;&gt;</param>
        /// <param name="arguments">The list of subtypes for the generic type, e.g string in List&lt;string&gt;</param>
        /// <param name="parameters"> List of parameters to pass to the constructor.</param>
        /// <returns></returns>
        public static object CreateGenericType(Type type, Type[] arguments, params  object[] parameters)
        {
            Type genericType = type.MakeGenericType(arguments);
            object obj;
            if (parameters != null && parameters.Count() > 0)
                obj = Activator.CreateInstance(genericType, parameters);
            else
                obj = Activator.CreateInstance(genericType);
            return obj;
        }

        public static Type GetGenericArgument(Type type)
        {
            Type[] types = type.GetGenericArguments();
            if (types.Count() > 1) throw new MapperException("Type {0} has more than one generic argument".Formatted(type.FullName));
            if (types.Count() == 0) throw new MapperException("The type {0} does not contain any generic arguments".Formatted(type.FullName));
            return types[0];
        }


        public static Field GetField(Item item, ID fieldId, string fieldName = "")
        {
            Field field = null;
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

        public static string ConstructQueryString(NameValueCollection parameters)
        {
            var sb = new StringBuilder();

            foreach (String name in parameters)
                sb.Append(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters[name]), "&"));

            if (sb.Length > 0)
                return sb.ToString(0, sb.Length - 1);

            return String.Empty;
        }

        public static Type GetGenericOuter(Type type)
        {
            return type.GetGenericTypeDefinition();
        }

        public static Item GetLanguageItem(Item foundItem, Language language)
        {
            if (foundItem == null) return null;

            var item = foundItem.Database.GetItem(foundItem.ID, language);
            if (item.Versions.Count > 0)
                return item;
            else
                return null;
        }
        public static IEnumerable<Item> GetLanguageItems(IEnumerable<Item> foundItems, Language language)
        {
            if (foundItems == null) return Enumerable.Empty<Item>();

            return foundItems.Select(x => Utilities.GetLanguageItem(x, language)).Where(x => x != null);
        }
    }
}



