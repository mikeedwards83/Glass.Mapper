using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreService : AbstractService<SitecoreDataContext>
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

            SitecoreDataContext context = new SitecoreDataContext();

            context.RequestedType = type;
            context.ConstructorParameters = null;
            context.Item = item;
            var obj = _factory.InstantiateObject(context);

            return obj;
        }
    }
}
