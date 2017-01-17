using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public class BuildManagerViewTypeResolver : IViewTypeResolver
    {
        public Type GetType(string path)
        {
            Type compiledViewType = BuildManager.GetCompiledType(path);
            Type baseType = compiledViewType.BaseType;

            if (baseType == null || !baseType.IsGenericType)
            {
                Sitecore.Diagnostics.Log.Warn(string.Format(
                    "View {0} compiled type {1} base type {2} does not have a single generic argument.",
                    path,
                    compiledViewType,
                    baseType), this);

                return typeof(NullModel);
            }

            Type proposedType = baseType.GetGenericArguments()[0];
            return proposedType == typeof(object)
                ? typeof(NullModel)
                : proposedType;
        }
    }
}
