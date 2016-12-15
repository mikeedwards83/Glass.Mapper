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
    public class RegexTypeFinder : ITypeFinder
    {
        private readonly IEnumerable<Regex> _regexPatterns;

        private static ConcurrentDictionary<string, Type> definedTypes;

        public static readonly Regex ModelRegex = new Regex(@"@model\s+(?<type>.*)\s?");
        public static readonly Regex InheritsRegex = new Regex("@inherits.*<(?<type>[^>]*)>");
        // public static readonly Regex StandardPattern = new Regex("GlassView<(?<type>.*)>|@model (?<type>[\\w\\.\\d]*)");
        private static IEnumerable<Assembly> _assemblies;


        static RegexTypeFinder()
        {
            _assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public RegexTypeFinder() : this(new[] { ModelRegex, InheritsRegex })
        {

        }
        public RegexTypeFinder(IEnumerable<Regex> regexPatterns)
        {
            _regexPatterns = regexPatterns;
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


            foreach (var regex in _regexPatterns)
            {
                var modelMatch = regex.Match(contents);
                if (modelMatch != null && modelMatch.Success)
                {
                    typeName = modelMatch.Groups["type"].Value;
                    break;
                }
            }

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

            if (foundTypes.Count > 1)
            {
                throw new AmbiguousMatchException("The type {0} exists in multiple assemblies".Formatted(typeName));
            }
            if (foundTypes.Count == 1)
            {
                return foundTypes.First();
            }

            return typeof(NullModel);

        }
    }

}
