using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using Sitecore.Configuration;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Xml.Xsl;
namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class Utilities
    /// </summary>
    public class Utilities : Mapper.Utilities
    {

        public static bool IsPageEditor
        {
            get
            {
#if SC82 || SC90 || SC91  || SC92  || SC93 || SC100 || SC1001
                return Sitecore.Context.PageMode.IsExperienceEditor;
#else
                return Sitecore.Context.PageMode.IsPageEditor;
#endif
            }
        }
        public static bool IsPageEditorEditing
        {
            get { return GetIsPageEditorEditing(); }
        }

        internal static Func<bool> GetIsPageEditorEditing = () =>
        {
#if SC82 || SC90 || SC91 || SC92 || SC93 || SC100 || SC1001
            return Sitecore.Context.PageMode.IsExperienceEditorEditing;
#else
            return Sitecore.Context.PageMode.IsPageEditorEditing;
#endif
        };


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
                sb.AppendFormat("{0}='{1}' ",key, attributes[key] ?? "");
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
                sb.Append($"{pair.Key}={quotationMark}{pair.Value ?? ""}{quotationMark} ");
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
            item.RuntimeSettings.Temporary = true;
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

        public class GlassImageRender : ImageRenderer
        {

            public Size GetFinalImageSize(Size imageSize, float imageScale, Size size, Size  maxSize )
            {
                return base.GetFinalImageSize(base.GetInitialImageSize(imageSize, imageScale, size), size, maxSize);
            }
        }
    }
}




