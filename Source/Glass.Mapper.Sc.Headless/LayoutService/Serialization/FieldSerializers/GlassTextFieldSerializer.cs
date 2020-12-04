using Glass.Mapper.Sc.Fields;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers
{
    public class GlassTextFieldSerializer : BaseGlassFieldSerializer
    {
        public GlassTextFieldSerializer(IGlassFieldRenderer fieldRenderer, ILog log)
            : base(fieldRenderer, log)
        {
        }

        public override void Serialize(GlassField field, JsonTextWriter writer)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            Assert.ArgumentNotNull((object)writer, nameof(writer));
            using (new SettingsSwitcher("Rendering.HtmlEncodedFieldTypes", string.Empty))
                base.Serialize(field, writer);
        }
    }
}