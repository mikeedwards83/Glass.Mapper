using System;
using System.Reflection;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Maintenance;

namespace Glass.Mapper.Sc.ContentSearch.LuceneProvider
{
    public class GlassLuceneIndex : LuceneIndex
    {
        public GlassLuceneIndex(string name, string folder, IIndexPropertyStore propertyStore) : base(name, folder, propertyStore)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            var fieldNameTranslatorField = typeof (LuceneIndex).GetField("fieldNameTranslator", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldNameTranslatorField == null) throw new Exception("Unable to set custom fieldNameTranslator");
            
            fieldNameTranslatorField.SetValue(this, new GlassLuceneFieldNameTranslator(this));
        }
    }
}
