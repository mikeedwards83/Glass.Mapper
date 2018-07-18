using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Builders;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
   
    public static class ISitecoreServiceExtensions
    {

        public static T GetItem<T>(this ISitecoreService service, Guid id)
            where T : class
        {
            return GetItem<T>(service, id, x => { });
        }

        public static T GetItem<T>(this ISitecoreService service, Guid id, Action<GetItemByIdBuilder> config) where T : class
        {
            var builder = new GetItemByIdBuilder().Id(id);
            config(builder);
            return service.GetItem<T>(builder);
        }

        public static T GetItem<T>(this ISitecoreService service, Item item)
            where T : class
        {
            return GetItem<T>(service, item, x => { });
        }

        public static T GetItem<T>(this ISitecoreService service, Item item, Action<GetItemByItemBuilder> config) where T : class
        {
            var builder = new GetItemByItemBuilder().Item(item);
            config(builder);
            return service.GetItem<T>(builder);
        }

        public static T GetItem<T>(this ISitecoreService service, string path)
            where T : class
        {
            return GetItem<T>(service, path, x => { });
        }

        public static T GetItem<T>(this ISitecoreService service, string path, Action<GetItemByPathBuilder> config) where T : class
        {
            var builder = new GetItemByPathBuilder().Path(path);
            config(builder);
            return service.GetItem<T>(builder);
        }

        public static T GetItem<T>(this ISitecoreService service, Query query)
            where T : class
        {
            return GetItem<T>(service, query, x => { });
        }

        public static T GetItem<T>(this ISitecoreService service, Query query, Action<GetItemByQueryBuilder> config) where T : class
        {
            var builder = new GetItemByQueryBuilder().Query(query.Value);
            config(builder);
            return service.GetItem<T>(builder);
        }

        public static T CreateItem<T>(this ISitecoreService service, object parent, string name) where T : class
        {
            return CreateItem<T>(service, parent, name, x => { });
        }

        public static T CreateItem<T>(this ISitecoreService service, object parent, string name, Action<CreateItemByNameBuilder> config) where T : class
        {
            var builder = new CreateItemByNameBuilder().Name(name).Parent(parent).Type(typeof(T));
            config(builder);
            return service.CreateItem<T>(builder);
        }


        public static T CreateItem<T>(this ISitecoreService service, object parent, T model) where T : class
        {
            return CreateItem<T>(service, parent, model, x => { });
        }

        public static T CreateItem<T>(this ISitecoreService service, object parent, T model, Action<CreateItemByModelBuilder> config) where T:class
        {
            var builder  = new CreateItemByModelBuilder().Model(model).Parent(parent);
            config(builder);
            return service.CreateItem<T>(builder);
        }

        public static void DeleteItem(this ISitecoreService service, object model)
        {
            DeleteItem(service, model, x => { });
        }

        public static void DeleteItem(this ISitecoreService service, object model, Action<DeleteItemByModelBuilder> config)
        {
            var builder = new DeleteItemByModelBuilder()
                .Model(model);

            config(builder);

            service.DeleteItem(builder);
        }

        public static IEnumerable<T> GetItems<T>(this ISitecoreService service,
            Func<Database, IEnumerable<Item>> func) where T : class
        {
            return GetItems<T>(service, func, x => { });
        }

        public static IEnumerable<T> GetItems<T>(this ISitecoreService service, Func<Database, IEnumerable<Item>> func, Action<GetItemsByFuncBuilder> config) where T:class
        {
            var builder = new GetItemsByFuncBuilder()
                .Func(func);
            config(builder);
            return service.GetItems<T>(builder);
        }

        public static IEnumerable<T> GetItems<T>(this ISitecoreService service, Query query) where T : class
        {
            return GetItems<T>(service, query, x => { });
        }

        public static IEnumerable<T> GetItems<T>(this ISitecoreService service, Query query, Action<GetItemsByQueryBuilder> config) where T : class
        {
            var builder = new GetItemsByQueryBuilder()
                .Query(query.Value);

            config(builder);
            return service.GetItems<T>(builder);
        }
        
        public static void MoveItem<T, TK>(this ISitecoreService service, T model, TK newParent)
        {
             MoveItem(service, model, newParent, x => { });
        }
        public static void MoveItem<T, TK>(this ISitecoreService service, T model, TK newParent, Action<MoveItemByModelBuilder> config)
        {
            var builder =  new MoveItemByModelBuilder()
                .Model(model)
                .NewParent(newParent);

            config(builder);

            service.MoveItem(builder);
        }

        public static void SaveItem(this ISitecoreService service, object model)
        {
            SaveItem(service, model, x => { });
        }
        public static void SaveItem(this ISitecoreService service, object model, Action<SaveItemByModelBuilder> config)
        {
            var builder =  new SaveItemByModelBuilder()
                .Model(model);

            config(builder);

            service.SaveItem(builder);
        }
    }
}
