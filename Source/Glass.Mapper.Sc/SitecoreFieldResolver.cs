using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreFieldResolver : ISitecoreFieldResolver
    {
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Field.</returns>
        public virtual Field GetField(Item item, string fieldName)
        {
            ValidateItem(item);
            return item.Fields[fieldName];
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldId">Id of the field.</param>
        /// <returns>Field.</returns>
        public virtual Field GetField(Item item, ID fieldId)
        {
            ValidateItem(item);

            if (ID.IsNullOrEmpty(fieldId))
            {
                return null;
            }

            return item.Fields[fieldId];
        }

        protected virtual void ValidateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item is null");
            }
        }

        public virtual Field GetSitecoreField(Item item, SitecoreFieldConfiguration scConfig)
        {
            Field field = GetField(item, scConfig.FieldId);

            if (field != null)
            {
                return field;
            }

            field = GetField(item, scConfig.FieldName);
            if (field != null)
            {
                return field;
            }

            // NM: Seperated to reduce the level of overrides that would be required from extension
            return GetAlternativeSitecoreField(item, scConfig);
        }

        protected virtual Field GetAlternativeSitecoreField(Item item, SitecoreFieldConfiguration scConfig)
        {
            string spaceFieldName = GetFieldName(scConfig.FieldName);
            Field field = GetField(item, spaceFieldName);
            if (field != null)
            {
                scConfig.FieldName = spaceFieldName;
            }

            return field;
        }

        protected virtual string GetFieldName(string fieldName)
        {
            return new string(InsertSpacesBeforeCaps(fieldName).ToArray()).Trim();
        }

        private IEnumerable<char> InsertSpacesBeforeCaps(IEnumerable<char> input)
        {
            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    yield return ' ';
                }

                yield return c;
            }
        }
    }
}
