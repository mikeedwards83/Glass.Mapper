using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;

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

        public T GetItem<T>(Guid Id)
        {
            SitecoreDataContext context = new SitecoreDataContext();
            context.RequestedType = typeof (T);
            context.ConstructorParameters = null;

            var obj =  _factory.InstantiateObject(context);

            return (T) obj;
        }
    }
}
