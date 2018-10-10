
using System;
using System.Linq.Expressions;
using Sitecore.Data;
namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that a query should be execute to load data into the property.
    /// The query can be either absolute or relative to the current item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreQuery<T> : AbstractPropertyBuilder<T, SitecoreQueryConfiguration>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreQuery{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreQuery(Expression<Func<T, object>> ex):base(ex)
        {
        }

     
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <returns>SitecoreQuery{`0}.</returns>
        public SitecoreQuery<T> InferType()
        {
            Configuration.InferType = true;
            return this;
        }
       
        /// <summary>
        /// Indicates that the field is relative to the current item.
        /// </summary>
        public SitecoreQuery<T> IsRelative()
        {
            Configuration.IsRelative = true;
            return this;
        }
        /// <summary>
        /// The query to execute
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>SitecoreQuery{`0}.</returns>
        public SitecoreQuery<T> Query(string query)
        {
            Configuration.Query = query;
            return this;
        }


        /// <summary>
        /// Enforce a template when mapping the property.
        /// </summary>
        /// <param name="templateId">The ID of the template that the item must use.</param>
        /// <param name="enforceTemplate">The type of template check to perform.</param>
        /// <returns></returns>
        public SitecoreQuery<T> TemplateId(ID templateId, SitecoreEnforceTemplate enforceTemplate)
        {
            Configuration.TemplateId = templateId;
            Configuration.EnforceTemplate = enforceTemplate;
            return this;
        }


    }
}




