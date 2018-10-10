using System.Collections.Specialized;
using Sitecore.Collections;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class ExtensionMethods
    /// </summary>
    public static class ExtensionMethods
    {
       #region NameValueCollection

        public static SafeDictionary<string> ToSafeDictionary(this NameValueCollection collection)
        {
            var safeDictionary = new SafeDictionary<string>();
            
            if (collection != null)
            {
                foreach (var key in collection.AllKeys)
                {
                    safeDictionary.Add(key, collection[key]);
                }
            }

            return safeDictionary;
        }

        #endregion

        public static bool HasValue(this Glass.Mapper.Sc.Fields.Image image)
        {
            return image != null && image.Src.HasValue();
        }
        public static bool HasValue(this Glass.Mapper.Sc.Fields.Link link)
        {
            return link != null && link.Url.HasValue();
        }
    }
}




