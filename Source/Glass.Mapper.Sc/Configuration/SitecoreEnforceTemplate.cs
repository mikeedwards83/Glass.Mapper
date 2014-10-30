using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Configuration
{
    public enum SitecoreEnforceTemplate
    {
        /// <summary>
        /// Will not enforce template check
        /// </summary>
        No,
        /// <summary>
        /// Checks only the items template
        /// </summary>
        Template,
        /// <summary>
        /// Checks the items template and any base templates
        /// </summary>
        TemplateAndBase

    }
}
