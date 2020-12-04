using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Pipelines.GetFieldSerializer;
using Glass.Mapper.Sc.Services;

namespace Glass.Mapper.Sc.LayoutService.Serialization.ItemSerializers
{
    public class DefaultGlassItemSerializer : BaseGlassItemSerializer
    {
        public DefaultGlassItemSerializer(
            IGlassFieldService glassFieldService, 
            IGetGlassFieldSerializerPipeline getFieldSerializerPipeline) 
            : base(glassFieldService, getFieldSerializerPipeline)
        {
        }
        public bool AlwaysIncludeEmptyFields { get; set; }

        protected internal override bool FieldFilter(GlassField field)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            return !field.Name.StartsWith("__", StringComparison.Ordinal);
        }

        protected virtual IEnumerable<GlassField> GetItemFields<T>(T item) where T : class
        {
            var itemFields = GlassFieldService.GetItemFields(item);
            return itemFields.Where<GlassField>(new Func<GlassField, bool>(((BaseGlassItemSerializer)this).FieldFilter));
        }

        protected override bool ShouldSerializeRenderedValue => Sitecore.Context.PageMode.IsExperienceEditorEditing;

       
    }
}