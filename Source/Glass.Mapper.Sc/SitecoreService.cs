using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    public class SitecoreService
    {
        private Database _database;
        private Context _context;

        public SitecoreService(Database database)
        {
            _database = database;
        }

        public SitecoreService()
        {
            _context = Context.Default;
        }

        //public T GetItem<T>(Guid Id)
        //{
        //    //var config = _context[typeof (T)];

        //    //var constructor = config.ConstructorMethods.First(x => x.Key.GetParameters().Count() == 0);

        //    return (T)null;

        //}
    }
}
