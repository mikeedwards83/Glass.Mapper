using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreService : AbstractService<SitecoreTypeContext, SitecoreDataMappingContext>
    {
        private Database _database;

        public SitecoreService(Database database, string contextName = "Default")
            :base(contextName)
        {
            _database = database;
        }

        public SitecoreService():base()
        {
        }

        public T GetItem<T>(Guid id) where T:class 
        {
            var item = _database.GetItem(new ID(id));
            return CreateClass(typeof (T), item) as T;

        }

        private object CreateClass(Type type, Item item)
        {
            if (item == null) return null;

            SitecoreTypeContext context = new SitecoreTypeContext();

            context.RequestedType = type;
            context.ConstructorParameters = null;
            context.Item = item;
            var obj = InstantiateObject(context);

            return obj;
        }

        public override AbstractDataMappingContext CreateDataMappingContext(ITypeContext typeContext, Object obj)
        {
            var scTypeContext = typeContext as SitecoreTypeContext;
            return new SitecoreDataMappingContext(obj, scTypeContext.Item);
        }
    }
}
