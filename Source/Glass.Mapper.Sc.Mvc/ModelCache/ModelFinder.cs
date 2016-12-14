using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using Glass.Mapper.Sc.Pipelines.Response;

namespace Glass.Mapper.Sc.ModelCache
{
    public class ModelFinder
    {
        private static ConcurrentDictionary<string, Type> definedTypes;

        private Regex modelPatternRegex;

        static ModelFinder()
        {
            definedTypes = new ConcurrentDictionary<string, Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var typeInfo in assembly.DefinedTypes)
                {
                    definedTypes.TryAdd(typeInfo.FullName, typeInfo.UnderlyingSystemType);
                }
            }
        }

        public virtual string ModelPattern
        {
            get { return "GlassView<(?<model>.*)>|@model (?<model>[\\w\\.\\d]*)"; }
        }

        public Regex ModelPatternRegex
        {
            get
            {
                if (modelPatternRegex == null)
                {
                    modelPatternRegex = new Regex(ModelPattern);
                }

                return modelPatternRegex;
            }
        }

        public virtual Type GetModelFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return typeof(NullModel);
            }

            string input = File.ReadAllText(path);

            MatchCollection matches = ModelPatternRegex.Matches(input);
            if (matches.Count != 1)
            {
                return typeof(NullModel);
            }

            string typeName = matches[0].Groups["model"].Value;

            if (definedTypes.ContainsKey(typeName))
            {
                return definedTypes[typeName];
            }

            return typeof(NullModel);
        }

        public virtual Type GetModel(string renderingItemPath, string path)
        {
            Type result = GetModelFromFile(path);
            if (result != typeof(NullModel))
            {
                return result;
            }

            return GetModelFromCompiled(renderingItemPath, path);
        }

        public virtual Type GetModelFromCompiled(string renderingItemPath, string path)
        {
            Type fileType = GetModelFromFile(path);

            if (fileType != typeof(NullModel))
            {
                return fileType;
            }

            Type compiledViewType = BuildManager.GetCompiledType(path);
            Type baseType = compiledViewType.BaseType;

            if (baseType == null || !baseType.IsGenericType)
            {
                Sitecore.Diagnostics.Log.Warn(string.Format(
                    "View {0} compiled type {1} base type {2} does not have a single generic argument.",
                    renderingItemPath,
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
