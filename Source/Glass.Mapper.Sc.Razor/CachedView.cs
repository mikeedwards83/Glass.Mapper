using System;
using RazorEngine.Templating;

namespace Glass.Mapper.Sc.Razor
{

    public class CachedView
    {
        public string ViewContent { get; set; }
        public ITemplate Template { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
    }
}
