using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.LayoutService.Serialization;

namespace Glass.Mapper.Sc.Pipelines.GetFieldSerializer
{
    public abstract class BaseGetGlassFieldSerializer : IGetGlassFieldSerializerProcessor
    {
        protected readonly IGlassFieldRenderer FieldRenderer;
        protected readonly ILog Log;

        public List<Type> FieldTypes { get; set; } = new List<Type>();

        protected BaseGetGlassFieldSerializer(IGlassFieldRenderer fieldRenderer, ILog log)
        {
            Assert.ArgumentNotNull((object)fieldRenderer, nameof(fieldRenderer));
            this.FieldRenderer = fieldRenderer;
            this.Log = log;
        }

        public virtual bool CanHandleField(GlassField field)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            return field != null && this.FieldTypes.Any<Type>((Func<Type, bool>)(f => f == field.Type));
        }

        public virtual void Process(GetGlassFieldSerializerPipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            if (!this.CanHandleField(args.Field))
                return;
            this.SetResult(args);
            args.AbortPipeline();
        }

        protected abstract void SetResult(GetGlassFieldSerializerPipelineArgs args);
    }
}