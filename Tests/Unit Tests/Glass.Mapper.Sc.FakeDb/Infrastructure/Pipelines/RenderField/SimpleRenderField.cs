using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Pipelines.RenderField;
using Sitecore.Xml.Xsl;

namespace Glass.Mapper.Sc.FakeDb.Infrastructure.Pipelines.RenderField
{
    public class SimpleRenderField
    {
        public const string ReplacementValue = "RenderedField";
        public const string ReplacementKey = "{renderField}";
        public void Process(RenderFieldArgs args)
        {
            args.Result = new RenderFieldResult(args.FieldValue.Replace(ReplacementKey, ReplacementValue));
        }
    }

}
