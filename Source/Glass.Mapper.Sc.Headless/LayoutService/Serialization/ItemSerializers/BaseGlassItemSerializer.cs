using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers;
using Glass.Mapper.Sc.Pipelines.GetFieldSerializer;
using Glass.Mapper.Sc.Services;
using Newtonsoft.Json;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Serialization;

namespace Glass.Mapper.Sc.LayoutService.Serialization.ItemSerializers
{
    public abstract class BaseGlassItemSerializer : IGlassItemSerializer
    {
        protected readonly IGlassFieldService GlassFieldService;
        protected readonly IGetGlassFieldSerializerPipeline GetFieldSerializerPipeline;

        protected BaseGlassItemSerializer(
            IGlassFieldService glassFieldService,
            IGetGlassFieldSerializerPipeline getFieldSerializerPipeline)
        {
            //Sitecore.LayoutService.Serialization.Pipelines.GetFieldSerializer.BaseGetFieldSerializer
            //Sitecore.LayoutService.Serialization.Pipelines.GetFieldSerializer.GetDefaultFieldSerializer


            Assert.ArgumentNotNull((object)getFieldSerializerPipeline, nameof(getFieldSerializerPipeline));
            this.GetFieldSerializerPipeline = getFieldSerializerPipeline;
            GlassFieldService = glassFieldService;
        }


        public virtual string Serialize<T>(T item) where T : class => this.SerializeItem(item, (SerializationOptions) null);

        public virtual string Serialize<T>(T item, SerializationOptions options) where T : class => this.SerializeItem(item, options);
        protected internal abstract bool FieldFilter(GlassField arg);

        protected virtual IEnumerable<GlassField> GetItemFields<T>(T item) where T : class
        {
            var itemFields = GlassFieldService.GetItemFields(item);
            return itemFields.Where<GlassField>(new Func<GlassField, bool>(this.FieldFilter));
        }

        protected virtual string SerializeItem<T>(T item, SerializationOptions options) where T : class
        {
            Assert.ArgumentNotNull((T) item, nameof(item));
            int currentDepth = this.SaveAndGetCurrentDepth();
            using (StringWriter stringWriter = new StringWriter())
            {
                using (JsonTextWriter writer = new JsonTextWriter((TextWriter) stringWriter))
                {
                    writer.WriteStartObject();
                    foreach (var itemField in GetItemFields(item))
                    {
                        this.SerializeField(itemField, writer, options, currentDepth);
                    }

                    writer.WriteEndObject();
                }

                this.RemoveDepth();
                return stringWriter.ToString();
            }
        }

        protected virtual void SerializeField(
            GlassField field,
            JsonTextWriter writer,
            SerializationOptions options,
            int depth)
        {
            IGlassFieldSerializer fieldSerializer = this.GetFieldSerializer(field);
            fieldSerializer.EnableRenderedValues = (options == null || !options.DisableEditing) && this.ShouldSerializeRenderedValue;
            fieldSerializer.Serialize(field, writer);
        }

        protected virtual IGlassFieldSerializer GetFieldSerializer(GlassField field)
        {
            IGlassFieldSerializer result = this.GetFieldSerializerPipeline.GetResult(new GetGlassFieldSerializerPipelineArgs()
            {
                Field = field,
                ItemSerializer = (IGlassItemSerializer)this
            });
            Assert.IsNotNull((object)result, "fieldSerializer != null");
            return result;
        }

        protected virtual bool ShouldSerializeRenderedValue => true;


        private void RemoveDepth() => HttpContext.Current.Items.Remove((object) "depth");

        private int SaveAndGetCurrentDepth()
        {
            int num1 = 0;
            if (HttpContext.Current.Items.Contains((object)"depth"))
                num1 = (int)HttpContext.Current.Items[(object)"depth"];
            int num2 = num1 + 1;
            HttpContext.Current.Items[(object)"depth"] = (object)num2;
            HttpContext.Current.Items[(object)"refdepth"] = (object)num2;
            return num2;
        }
    }
}