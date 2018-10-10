using System;
using System.Linq.Expressions;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the property should pull from the item links list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreLinked<T> : AbstractPropertyBuilder<T, SitecoreLinkedConfiguration>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreLinked{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public SitecoreLinked(Expression<Func<T, object>> ex)
            : base(ex)
        {
        }
       
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <returns>SitecoreLinked{`0}.</returns>
        public SitecoreLinked<T> InferType()
        {
            
            Configuration.InferType = false;
            return this;
        }
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>SitecoreLinked{`0}.</returns>
        public SitecoreLinked<T> Option(SitecoreLinkedOptions option)
        {
            Configuration.Option = option;
            return this;
        }

        /// <summary>
        /// Enforce a template when mapping the property.
        /// </summary>
        /// <param name="templateId">The ID of the template that the item must use.</param>
        /// <param name="enforceTemplate">The type of template check to perform.</param>
        /// <returns></returns>
        public SitecoreLinked<T> TemplateId(ID templateId, SitecoreEnforceTemplate enforceTemplate)
        {
            Configuration.TemplateId = templateId;
            Configuration.EnforceTemplate = enforceTemplate;
            return this;
        }
    }
}




