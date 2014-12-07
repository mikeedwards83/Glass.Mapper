using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.Dynamic
{
    public interface IDynamicItem: IDynamicMetaObjectProvider
    {
        string ContentPath { get; }
        string DisplayName { get; }
        string FullPath { get; }
        string Key { get; }
        string MediaUrl { get; }
        string Path { get; }
        Guid TemplateId { get; }
        string TemplateName { get; }
        string Url { get; }
        int Version { get; }
        string Name { get; }
        Language Language { get; }
        IEnumerable<Guid> BaseTemplateIds { get; }
    }
}
