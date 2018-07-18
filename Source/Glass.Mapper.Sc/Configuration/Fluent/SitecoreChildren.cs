using System;
using System.Linq.Expressions;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the property should contain the children of the current item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreChildren<T> : AbstractPropertyBuilder<T, SitecoreChildrenConfiguration>
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreChildren{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreChildren(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }
       
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <returns>SitecoreChildren{`0}.</returns>
        public SitecoreChildren<T> InferType()
        {
            Configuration.InferType = true;
            return this;
        }

        /// <summary>
        /// Check the template of the mapped item matches either the template ID defined on the property
        /// or the model template ID.
        /// </summary>
        /// <returns>SitecoreChildren{`0}.</returns>
        public SitecoreChildren<T> EnforceTemplate()
        {
            Configuration.EnforceTemplate = SitecoreEnforceTemplate.Template;
            return this;
        }

        /// <summary>
        /// Check the template of the mapped item matches either the template ID defined on the property
        /// or the model template ID.
        /// </summary>
        /// <returns>SitecoreChildren{`0}.</returns>
        public SitecoreChildren<T> EnforceTemplateAndBase()
        {
            Configuration.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;
            return this;
        }

        public SitecoreChildren<T> TemplateId(ID templateId, SitecoreEnforceTemplate enforceTemplate)
        {
            Configuration.TemplateId = templateId;
            Configuration.EnforceTemplate = enforceTemplate;
            return this;
        }

    }
}




