using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class Config 
    {

        /// <summary>
        /// Indicates that classes should be build using the Windsor dependency resolver. Default is False
        /// </summary>
        /// <value>
        /// <c>true</c> if [use windsor contructor]; otherwise, <c>false</c>.
        /// </value>
        public bool UseIoCConstructor { get; set; }

		/// <summary>
		/// Indicates that all mapped classes should import the base class' mappings as well
		/// </summary>
		public bool AutoImportBaseClasses { get; set; }

		public DebugSettings Debug { get; private set; }

        /// <summary>
        /// When set to false lazy loading will be disabled for all properties that reference other Glass models. This 
        /// applies to all models that are also generated as part of the parent model. 
        /// Default: false
        /// </summary>
        public bool EnableLazyLoadingForCachableModels { get; set; }

        public Config()
        {
            Debug = new DebugSettings();
            EnableLazyLoadingForCachableModels = false;
        }
        public class DebugSettings
        {
            public DebugSettings()
            {
                SlowModelThreshold = 100;
            }
            public bool Enabled { get; set; }
            

            /// <summary>
            /// Time in milliseconds
            /// </summary>
            public int SlowModelThreshold { get; set; }
        }
    }
}
