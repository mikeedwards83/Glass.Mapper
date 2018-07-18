using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Configuration
{
    public enum SitecoreEnforceTemplate
    {
        /// <summary>
        /// Default, Will not enforce template check
        /// </summary>
        Default = 0,
        /// <summary>
        /// Will not enforce template check
        /// </summary>
        No = 5,
        /// <summary>
        /// Checks only the items template
        /// </summary>
        Template = 10,
        /// <summary>
        /// Checks the items template and any base templates
        /// </summary>
        TemplateAndBase = 15

    }

    public static class SitecoreEnforceTemplateExtensions
    {
        /// <summary>
        /// Indicates that some form of template enforcement is enabled. Check the Enum to verify what type of
        /// enforcement
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEnabled(this SitecoreEnforceTemplate value)
        {
            return value != SitecoreEnforceTemplate.Default && value != SitecoreEnforceTemplate.No;
        }
    }
}
