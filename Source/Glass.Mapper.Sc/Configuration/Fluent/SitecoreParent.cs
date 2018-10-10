using System;
using System.Linq.Expressions;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the property should be populated with the parent item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreParent<T> : AbstractPropertyBuilder<T, SitecoreParentConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreParent{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreParent(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }

       
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <returns>SitecoreParent{`0}.</returns>
        public SitecoreParent<T> InferType()
        {
            Configuration.InferType = true;
            return this;
        }


        /// <summary>
        /// Enforce a template when mapping the property.
        /// </summary>
        /// <param name="templateId">The ID of the template that the item must use.</param>
        /// <param name="enforceTemplate">The type of template check to perform.</param>
        /// <returns></returns>
        public SitecoreParent<T> TemplateId(ID templateId, SitecoreEnforceTemplate enforceTemplate)
        {
            Configuration.TemplateId = templateId;
            Configuration.EnforceTemplate = enforceTemplate;
            return this;
        }



    }
}




