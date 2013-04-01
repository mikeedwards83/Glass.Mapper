using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core;
using umbraco.editorControls;
using umbraco.interfaces;

namespace Glass.Mapper.Umb.Integration
{
    /// <summary>
    /// Extends the CoreBootManager for use with testing
    /// </summary>
    public class TestBootManager : CoreBootManager
    {
        public TestBootManager(UmbracoApplicationBase umbracoApplication)
            : base(umbracoApplication)
        {
            //This is only here to ensure references to the assemblies needed for the DataTypesResolver
            //otherwise they won't be loaded into the AppDomain.
            var interfacesAssemblyName = typeof(IDataType).Assembly.FullName;
            var editorControlsAssemblyName = typeof(uploadField).Assembly.FullName;
        }
    }
}
