using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor.Model
{
    [SitecoreType(TemplateId="{4432051D-8D3E-48E9-8C06-F1970EE607C5}")]
    public class GlassDynamicRazor
    {

        [SitecoreInfo(Configuration.SitecoreInfoType.Name)]
        public string Name { get; set; }
        
        [SitecoreField("Name")]
        public string View { get; set; }
    }
}
