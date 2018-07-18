using System;
using System.Linq.Expressions;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the property should be populated with the composite item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreSelf<T> : AbstractPropertyBuilder<T, SitecoreSelfConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreSelf{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreSelf(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }

     
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <returns>SitecoreSelf{`0}.</returns>
        public SitecoreSelf<T> InferType()
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
        public SitecoreSelf<T> TemplateId(ID templateId, SitecoreEnforceTemplate enforceTemplate)
        {
            Configuration.TemplateId = templateId;
            Configuration.EnforceTemplate = enforceTemplate;
            return this;
        }

    }
}




