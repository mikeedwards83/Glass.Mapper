using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreService : AbstractService<SitecoreTypeContext, SitecoreDataMappingContext>, ISitecoreService
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

        public void Save<T>(T obj)
        {
            //TODO: should this be a separate context
          //  SitecoreTypeContext context = new SitecoreTypeContext();

            //TODO: ME - this may not work with a proxy
            var config = GlassContext[obj.GetType()] as SitecoreTypeConfiguration;
            var item = config.ResolveItem(obj, _database);
            if(item == null)
                throw new MapperException("Could not save class, item not found");

            

           


        }

        private object CreateClass(Type type, Item item)
        {
            if (item == null) return null;

            SitecoreTypeContext context = new SitecoreTypeContext();
            context.SitecoreService = this;
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
