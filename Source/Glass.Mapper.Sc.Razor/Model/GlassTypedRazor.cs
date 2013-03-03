using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor.Model
{
    [SitecoreType(TemplateId = "{7B10C01D-B0DF-4626-BE34-F48E38828FB7}")]
    public class GlassTypedRazor
    {
        [SitecoreInfo(Configuration.SitecoreInfoType.Name)]
        public string Name { get; set; }

        [SitecoreField("Name")]
        public string View { get; set; }

        [SitecoreField]
        public string Code { get; set; }

        [SitecoreField]
        public string Type { get; set; }

        [SitecoreField]
        public string Assembly { get; set; }
    }
}
