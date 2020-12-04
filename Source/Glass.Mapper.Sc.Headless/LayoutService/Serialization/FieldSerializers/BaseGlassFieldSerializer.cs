using System;
using System.Web;
using System.Xml;
using Glass.Mapper.Sc.Fields;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Serialization;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;

namespace Glass.Mapper.Sc.LayoutService.Serialization.FieldSerializers
{
    public class BaseGlassFieldSerializer : IGlassFieldSerializer
    {
        private int? _maxDepth;
        protected readonly IGlassFieldRenderer FieldRenderer;
        protected readonly BaseFactory BaseFactory = ServiceLocator.ServiceProvider.GetService<BaseFactory>();
        private ILog _log;

        protected int SerializationMaxDepth
        {
            get
            {
                if (!this._maxDepth.HasValue)
                    this._maxDepth = new int?(this.GetMaxSerializationDepth());
                return this._maxDepth.Value;
            }
        }

        protected BaseGlassFieldSerializer(IGlassFieldRenderer fieldRenderer, ILog log)
        {
            Assert.ArgumentNotNull((object)fieldRenderer, nameof(fieldRenderer));
            this.FieldRenderer = fieldRenderer;
            _log = log;
        }

        protected virtual ItemUrlBuilderOptions UrlOptions
        {
            get
            {
                ItemUrlBuilderOptions urlBuilderOptions = LinkManager.GetDefaultUrlBuilderOptions();
                urlBuilderOptions.LanguageEmbedding = new LanguageEmbedding?(LanguageEmbedding.Never);
                urlBuilderOptions.AlwaysIncludeServerUrl = new bool?(false);
                urlBuilderOptions.SiteResolving = new bool?(true);
                return urlBuilderOptions;
            }
        }

        protected virtual FieldRendererResult RenderField(
          GlassField field,
          bool disableEditing = false)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            try
            {
                return this.FieldRenderer.RenderField(field, disableEditing);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Error whole rendering field {0}", (object)field.Name));
                return new FieldRendererResult(string.Empty, string.Empty);
            }
        }

        public virtual void Serialize(GlassField field, JsonTextWriter writer)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            Assert.ArgumentNotNull((object)writer, nameof(writer));
            if (!string.IsNullOrWhiteSpace(field.Value))
                _log.Debug(string.Format("Attempting to load value for field {0}", (object)field.Name));
            writer.WritePropertyName(field.Name);
            writer.WriteStartObject();
            writer.WritePropertyName("value");
            this.WriteValue(field, writer);
            if (this.EnableRenderedValues)
            {
                writer.WritePropertyName("editable");
                this.WriteRenderedValue(field, writer);
            }
            this.WriteExtendedValues(field, writer);
            writer.WriteEndObject();
        }

        public bool EnableRenderedValues { get; set; }

        protected virtual void WriteValue(GlassField field, JsonTextWriter writer)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            Assert.ArgumentNotNull((object)writer, nameof(writer));
            writer.WriteValue(field.Value);
        }

        protected virtual void WriteRenderedValue(GlassField field, JsonTextWriter writer)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            Assert.ArgumentNotNull((object)writer, nameof(writer));
            FieldRendererResult fieldRendererResult = this.RenderField(field);
            writer.WriteValue(fieldRendererResult.ToString());
        }

        /// <summary>
        /// Write additional key/value pairs to the field object (or other data)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="writer"></param>
        protected virtual void WriteExtendedValues(GlassField field, JsonTextWriter writer)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            Assert.ArgumentNotNull((object)writer, nameof(writer));
        }

        protected int GetMaxSerializationDepth()
        {
            XmlNode configNode = this.BaseFactory.GetConfigNode("layoutService/serializationMaxDepth");
            int result;
            return configNode != null && int.TryParse(configNode.InnerText, out result) ? result : 4;
        }

        protected void SaveDepth(int depth)
        {
            if (HttpContext.Current == null)
                return;
            if (HttpContext.Current.Items.Contains((object)Sitecore.LayoutService.Constants.Depth))
                HttpContext.Current.Items.Remove((object)Sitecore.LayoutService.Constants.Depth);
            HttpContext.Current.Items.Add((object)Sitecore.LayoutService.Constants.Depth, (object)depth);
        }

        protected int GetDepth() => HttpContext.Current != null && HttpContext.Current.Items.Contains((object)Sitecore.LayoutService.Constants.RefDepth) ? (int)HttpContext.Current.Items[(object)Sitecore.LayoutService.Constants.RefDepth] : 0;
    }
}