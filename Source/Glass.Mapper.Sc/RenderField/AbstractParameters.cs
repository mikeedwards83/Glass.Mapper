using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Sitecore.Text;

namespace Glass.Mapper.Sc.RenderField
{
    /// <summary>
    /// Base class for rendering parameters that will be passed to the renderField pipeline.
    /// </summary>
    public class AbstractParameters
    {
        private readonly NameValueCollection _parameters = new NameValueCollection();

        protected NameValueCollection Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public override string ToString()
        {
            var url = new UrlString();
            url.Parameters.Add(Parameters);
            return url.Query;
        }
    }
}
