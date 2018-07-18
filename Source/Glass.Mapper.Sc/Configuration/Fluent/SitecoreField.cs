using System;
using System.Linq.Expressions;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Used to populate the property with data from a Sitecore field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreField<T> : AbstractPropertyBuilder<T, SitecoreFieldConfiguration>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreField{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreField(Expression<Func<T, object>> ex)
            : base(ex)
        {
            Configuration.FieldName = Configuration.PropertyInfo.Name;
        }

        /// <summary>
        /// The ID of the field to load or create in code first
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreField<T> FieldId(string id)
        {
            return FieldId(new ID(id));
        }

        /// <summary>
        /// The ID of the field to load or create in code first
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreField<T> FieldId(Guid id)
        {
            return FieldId(new ID(id));
        }

        /// <summary>
        /// The ID of the field to load or create in code first
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreField<T> FieldId(ID id)
        {
            Configuration.FieldId = id;
            return this;
        }


        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreField<T> FieldName(string name)
        {
            Configuration.FieldName = name;
            return this;
        }

        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreField<T> Setting(SitecoreFieldSettings setting)
        {
            Configuration.Setting = setting;
            return this;
        }

        /// <summary>
        /// Indicate that the field can not be written to Sitecore
        /// </summary>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreField<T> ReadOnly()
        {
            Configuration.ReadOnly = true;
            return this;
        }

        public SitecoreField<T> MediaUrlOptions(SitecoreMediaUrlOptions options)
        {
            Configuration.MediaUrlOptions = options;
            return this;
        }

    }
}



