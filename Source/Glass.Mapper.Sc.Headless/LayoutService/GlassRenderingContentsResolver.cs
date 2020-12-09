using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;

namespace Glass.Mapper.Sc.LayoutService
{
    /// <summary>
    /// Glass Rendering Contents Resolver
    /// </summary>
    public class GlassRenderingContentsResolver<T> : IRenderingContentsResolver where T : class
    {
        ISitecoreService SitecoreService { get; }

        public bool IncludeServerUrlInMediaUrls { get; set; } = true;
        public bool UseContextItem { get; set; }
        public string ItemSelectorQuery { get; set; }
        public NameValueCollection Parameters { get; set; } = new NameValueCollection(0);

        public GlassRenderingContentsResolver() : this(new SitecoreService(Sitecore.Context.Database))
        {
        }

        /// <summary>
        /// Glass Rendering Contents Resolver Constructor 
        /// </summary>
        /// <param name="sitecoreService"></param>
        public GlassRenderingContentsResolver(ISitecoreService sitecoreService)
        {
            SitecoreService = sitecoreService;
        }

        public virtual object ResolveContents(
            Rendering rendering,
            IRenderingConfiguration renderingConfig) 
        {
            Assert.ArgumentNotNull((object)rendering, nameof(rendering));
            Assert.ArgumentNotNull((object)renderingConfig, nameof(renderingConfig));
            T contextItem = this.GetContextItem(rendering, renderingConfig);
            if (contextItem == null)
                return (object)null;
            if (string.IsNullOrWhiteSpace(this.ItemSelectorQuery))
                return (object)this.ProcessItem(contextItem, rendering, renderingConfig);

            JObject jobject = new JObject()
            {
                ["items"] = (JToken)new JArray()
            };
            IEnumerable<T> items = this.GetItems<T>(contextItem);
            List<T> objList = items != null ? items.ToList<T>() : (List<T>)null;
            if (objList == null || objList.Count == 0)
                return (object)jobject;
            jobject["items"] = (JToken)this.ProcessItems((IEnumerable<T>)objList, rendering, renderingConfig);
            return (object)jobject;
        }

        protected virtual IEnumerable<T> GetItems<T>(T contextItem) where T : class
        {
          
            Assert.ArgumentNotNull((object)contextItem, nameof(contextItem));
            var options = new GetItemsByQueryOptions(Sc.Query.New(this.ItemSelectorQuery));
            return string.IsNullOrWhiteSpace(this.ItemSelectorQuery) ? Enumerable.Empty<T>() : SitecoreService.GetItems<T>(options);
        }

        /// <summary>
        /// Returns Content Rendering Context GlassMapper Object 
        /// </summary>
        /// <param name="rendering"></param>
        /// <param name="renderingConfig"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T GetContextItem(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var options = new GetKnownOptions();
            if (this.UseContextItem)
                options.Item = Sitecore.Context.Item;
            if (string.IsNullOrWhiteSpace(rendering.DataSource))
                return null;
            options.Item = rendering.RenderingItem?.Database.GetItem(rendering.DataSource);
            return SitecoreService.GetItem<T>(options);
        }

        protected virtual JArray ProcessItems<T>(
            IEnumerable<T> items,
            Rendering rendering,
            IRenderingConfiguration renderingConfig) where T : class
        {
            JArray jarray = new JArray();
            foreach (var obj in items)
            {
                var scItem = SitecoreService.ResolveItem(obj);

                JObject jobject1 = this.ProcessItem(obj, rendering, renderingConfig);
                JObject jobject2 = new JObject()
                {
                    ["id"] = (JToken)scItem.ID.Guid.ToString(),
                    ["name"] = (JToken)scItem.Name,
                    ["displayName"] = (JToken)scItem.DisplayName,
                    ["fields"] = (JToken)jobject1
                };
                jarray.Add((JToken)jobject2);
            }
            return jarray;
        }

        protected virtual JObject ProcessItem<T>(
            T item,
            Sitecore.Mvc.Presentation.Rendering rendering,
            IRenderingConfiguration renderingConfig) where T : class
        {
            Assert.ArgumentNotNull((object)item, nameof(item));
            using (new SettingsSwitcher("Media.AlwaysIncludeServerUrl", this.IncludeServerUrlInMediaUrls.ToString()))
            {
                //return JObject.Parse(GlassItemSerializer.Serialize(item));
                var scItem = SitecoreService.ResolveItem(item);
                return JObject.Parse(renderingConfig.ItemSerializer.Serialize(scItem));
            }
        }

    }
}