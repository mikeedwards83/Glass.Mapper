using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using umbraco;
using umbraco.NodeFactory;
using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public class UmbracoService : AbstractService, IUmbracoService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <param name="constructorParameters">Parameters to pass to the constructor of the new class. Must be in the order specified on the consturctor.</param>
        /// <returns></returns>
        public object CreateType(Type type, IContent content, bool isLazy, bool inferType, params object[] constructorParameters)
        {
            if (content == null) return null;


            if (constructorParameters != null && constructorParameters.Length > 4)
                throw new NotSupportedException("Maximum number of constructor parameters is 4");

            UmbracoTypeCreationContext creationContext = new UmbracoTypeCreationContext();
            creationContext.UmbracoService = this;
            creationContext.RequestedType = type;
            creationContext.ConstructorParameters = constructorParameters;
            creationContext.Content = content;
            creationContext.InferType = inferType;
            creationContext.IsLazy = isLazy;
            var obj = InstantiateObject(creationContext);

            return obj;
        }

        public T GetItem<T>(int id, bool isLazy = false, bool inferType = false) where T : class
        {
            var contentService = new ContentService();
            var item = contentService.GetById(id);
            return CreateType(typeof(T), item, isLazy, inferType) as T;
        }


        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T>(IContent content, bool isLazy = false, bool inferType = false) where T : class
        {
            return (T)CreateType(typeof(T), content, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K>(IContent content, K param1, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), content, isLazy, inferType, param1);

        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K, L>(IContent content, K param1, L param2, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), content, isLazy, inferType, param1, param2);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K, L, M>(IContent content, K param1, L param2, M param3, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), content, isLazy, inferType, param1, param2, param3);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, K, L, M, N>(IContent content, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), content, isLazy, inferType, param1, param2, param3, param4);
        }

        /// <summary>
        /// Creates a new Umbraco class. 
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the UmbracoClassAttribute or fluent equivalent</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the UmbracoIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute UmbracoInfoAttribute of type UmbracoInfoType.Name or the fluent equivalent</param>
        /// <returns></returns>
        public T Create<T>(int parent, T newItem) where T : class
        {
            var newType = (UmbracoTypeConfiguration)null;
            try
            {
                newType = GlassContext.GetTypeConfiguration(newItem) as UmbracoTypeConfiguration;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for new item type {0}".Formatted(typeof(T).FullName), ex);
            }


            var parentType = (UmbracoTypeConfiguration)null;
            try
            {
                parentType = GlassContext.GetTypeConfiguration(parent) as UmbracoTypeConfiguration;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for parent item type {0}".Formatted(typeof(int).FullName), ex);
            }


            var contentService = new ContentService();

            var pItem = parentType.ResolveItem(parent, contentService);

            if (pItem == null)
                throw new MapperException("Could not find parent item");


            var nameProperty = newType.Properties.Where(x => x is UmbracoInfoConfiguration)
                .Cast<UmbracoInfoConfiguration>().FirstOrDefault(x => x.Type == UmbracoInfoType.Name);

            if (nameProperty == null)
                throw new MapperException("The type {0} does not have a property with attribute UmbracoInfo(UmbracoInfoType.Name)".Formatted(newType.Type.FullName));
            
            string tempName = Guid.NewGuid().ToString();
            var content = contentService.CreateContent(tempName, pItem, newType.ContentTypeAlias);

            if (content == null) throw new MapperException("Failed to create item");

            //write new data to the item

            WriteToItem<T>(newItem, content);

            //then read it back

            UmbracoTypeCreationContext typeContext = new UmbracoTypeCreationContext();
            typeContext.Content = content;
            typeContext.UmbracoService = this;

            newType.MapPropertiesToObject(newItem, this, typeContext);

            return newItem;
        }

        public void WriteToItem<T>(T target, IContent content, UmbracoTypeConfiguration config = null)
        {
            if (config == null)
                config = GlassContext.GetTypeConfiguration(target) as UmbracoTypeConfiguration;

            UmbracoTypeSavingContext savingContext = new UmbracoTypeSavingContext();
            savingContext.Config = config;

            //ME-an item with no versions should be null

            savingContext.Content = content;
            savingContext.Object = target;
            
            SaveObject(savingContext);

            var contentService = new ContentService();
            contentService.Save(savingContext.Content);
        }

        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var umbTypeContext = abstractTypeCreationContext as UmbracoTypeCreationContext;
            return new UmbracoDataMappingContext(obj, umbTypeContext.Content, this, new ContentService());
        }

        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var umbContext = creationContext as UmbracoTypeSavingContext;
            return new UmbracoDataMappingContext(umbContext.Object, umbContext.Content, this, new ContentService());
        }

        
    }
}
