using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Azure.FieldMaps;
using Sitecore.ContentSearch.Azure.Http;
using Sitecore.ContentSearch.Azure.Schema;
using Sitecore.ContentSearch.Maintenance;

namespace Glass.Mapper.Sc.ContentSearch.AzureSearchProvider
{
    public class GlassAzureSearchIndex : CloudSearchProviderIndex
    {
        //WHY ARE BASE PROPERTIES DEFINED WITH PRIVATE SETTERS??
        public ICloudSearchIndexSchemaBuilder MySchemaBuilder
        {
            get { return base.SchemaBuilder; }
            set
            {
                typeof(CloudSearchProviderIndex).GetProperty("SchemaBuilder").GetSetMethod(true).Invoke(this, new object[] { value });
            }
        }

        public ISearchService MySearchService
        {
            get { return base.SearchService; }
            set
            {
                typeof(CloudSearchProviderIndex).GetProperty("SearchService").GetSetMethod(true).Invoke(this, new object[] { value });
            }
        }

        public GlassAzureSearchIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore) : base(name, connectionStringName, totalParallelServices, propertyStore)
        {
        }

        public GlassAzureSearchIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore, string @group) : base(name, connectionStringName, totalParallelServices, propertyStore, @group)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            this.FieldNameTranslator = new GlassAzureSearchFieldNameTranslator(this.Configuration.FieldMap.AsCloudFieldMap());
        }
    }
}
