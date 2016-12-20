using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public class RegexViewTypeResolver : IViewTypeResolver
    {
        private readonly IEnumerable<Regex> _modelPatterns;
        private readonly IEnumerable<Regex> _usingPatterns;


        public static readonly Regex UsingRegex = new Regex(@"@using\s+(?<namespace>[^\n\s]+)");
        public static readonly Regex ModelRegex = new Regex(@"@model\s+(?<type>[^\n\s]+)");
        public static readonly Regex InheritsRegex = new Regex("@inherits.*<(?<type>[^>]*)>");
        private static IEnumerable<Assembly> _assemblies;


        static RegexViewTypeResolver()
        {
            _assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public RegexViewTypeResolver() : this(
            new[] { ModelRegex, InheritsRegex },
            new [] {UsingRegex})
        {

        }
        public RegexViewTypeResolver(
            IEnumerable<Regex> modelPatterns,
            IEnumerable<Regex> usingPatterns)

        {
            _modelPatterns = modelPatterns;
            _usingPatterns = usingPatterns;
        }

        public virtual string GetContents(string path)
        {
            var fullPath = HttpContext.Current.Server.MapPath(path);
            if (!File.Exists(fullPath))
            {
                return string.Empty;
            }
            return File.ReadAllText(fullPath);
        }
        public Type GetType(string path)
        {
            var contents = GetContents(path);
            string typeName = String.Empty;


            foreach (var regex in _modelPatterns)
            {
                var modelMatch = regex.Match(contents);
                if (modelMatch != null && modelMatch.Success)
                {
                    typeName = modelMatch.Groups["type"].Value;
                    break;
                }
            }

            var foundTypes = FindTypes(typeName);

            if (!foundTypes.Any())
            {
                foreach (var regex in _usingPatterns)
                {
                    var matches = regex.Matches(contents);
                    foreach (Match match  in matches)
                    {
                        var ns = match.Groups["namespace"].Value;

                        var nsType = "{0}.{1}".Formatted(ns, typeName);
                        foundTypes = FindTypes(nsType);
                        if (foundTypes.Any())
                        {
                            break;
                        }
                    }
                }
            }

            if (foundTypes.Count() > 1)
            {
                throw new AmbiguousMatchException("The type {0} exists in multiple assemblies".Formatted(typeName));
            }

            if (foundTypes.Any())
            {
                return foundTypes.First();
            }

            return typeof(NullModel);

        }

        private IEnumerable<Type> FindTypes(string typeName)
        {
            List<Type> foundTypes = new List<Type>();
            if (!typeName.IsNullOrWhiteSpace())
            {
                foreach (var assembly in _assemblies)
                {
                    var foundType = assembly.GetType(typeName);
                    if (foundType != null)
                    {
                        foundTypes.Add(foundType);
                    }
                }
            }

            return foundTypes;
        }
    }

}
