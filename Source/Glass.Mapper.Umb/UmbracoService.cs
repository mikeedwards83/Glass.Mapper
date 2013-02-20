using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.NodeFactory;
using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public class UmbracoService : AbstractService, IUmbracoService
    {
        public T GetItem<T>(int id) where T:class 
        {
            var node = new Node(id);
            return CreateClass(typeof (T), node) as T;

        }

        public void Save<T>(T obj)
        {
          //  //TODO: should this be a separate context
          ////  SitecoreTypeContext context = new SitecoreTypeContext();

          //  //TODO: ME - this may not work with a proxy
          //  var config = GlassContext[obj.GetType()] as SitecoreTypeConfiguration;
          //  var item = config.ResolveItem(obj, Database);
          //  if(item == null)
          //      throw new MapperException("Could not save class, item not found");

          //  SitecoreTypeSavingContext savingContext = new SitecoreTypeSavingContext();
          //  savingContext.Config = config;
          //  savingContext.Item = item;
          //  savingContext.Object = obj;

          //  item.Editing.BeginEdit();

          //  SaveObject(savingContext);

          //  item.Editing.EndEdit();
        }

        public object CreateClass(Type type, INode node, bool isLazy = false, bool inferType = false)
        {
            if (node == null) return null;

            UmbracoTypeCreationContext creationContext = new UmbracoTypeCreationContext();
            creationContext.UmbracoService = this;
            creationContext.RequestedType = type;
            creationContext.ConstructorParameters = null;
            creationContext.Node = node;
            creationContext.InferType = inferType;
            creationContext.IsLazy = isLazy;
            var obj = InstantiateObject(creationContext);

            return obj;
        }

        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var umbTypeContext = abstractTypeCreationContext as UmbracoTypeCreationContext;
            return new UmbracoDataMappingContext(obj, umbTypeContext.Node, this);
        }

        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var umbContext = creationContext as UmbracoTypeSavingContext;
            return new UmbracoDataMappingContext(umbContext.Object, umbContext.Node, this);
        }

        
    }
}
