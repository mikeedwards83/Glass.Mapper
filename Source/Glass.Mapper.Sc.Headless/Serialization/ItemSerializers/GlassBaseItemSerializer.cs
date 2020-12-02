using Newtonsoft.Json;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Serialization.Pipelines.GetFieldSerializer;
using System.IO;
using System.Web;
using Sitecore.LayoutService.Serialization;

namespace Glass.Mapper.Sc.Serialization.ItemSerializers
{
    public abstract class GlassBaseItemSerializer : IGlassItemSerializer
    {

        readonly JsonSerializer _serializer = new JsonSerializer();


        protected GlassBaseItemSerializer()
        {
        }

        public virtual string Serialize<T>(T item) where T : class =>
            this.SerializeItem(item, (SerializationOptions) null);

        public virtual string Serialize<T>(T item, SerializationOptions options) where T : class =>
            this.SerializeItem(item, options);


        protected virtual string SerializeItem<T>(T item, SerializationOptions options) where T : class
        {
            Assert.ArgumentNotNull((object) item, nameof(item));
            int currentDepth = this.SaveAndGetCurrentDepth();
            using (StringWriter stringWriter = new StringWriter())
            {
                using (JsonTextWriter writer = new JsonTextWriter((TextWriter) stringWriter))
                {
                    writer.WriteStartObject();

                    // Serialize Item
                    _serializer.Serialize(writer, item);

                    writer.WriteEndObject();
                }

                this.RemoveDepth();
                return stringWriter.ToString();
            }
        }

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