using System;
using System.Reflection;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.SolrProvider;

namespace Glass.Mapper.Sc.ContentSearch.SolrProvider
{
    public class GlassSolrIndex : SolrSearchIndex
    {
        public GlassSolrIndex(string name, string folder, IIndexPropertyStore propertyStore) : base(name, folder, propertyStore)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            this.FieldNameTranslator = new GlassSolrFieldNameTranslator(this);
        }
    }
}
