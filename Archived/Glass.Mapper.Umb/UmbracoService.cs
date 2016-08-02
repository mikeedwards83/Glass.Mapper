/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-
using System;
using System.Linq;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// Class UmbracoService
    /// </summary>
    public class UmbracoService : AbstractService, IUmbracoService
    {
        /// <summary>
        /// Gets the content service.
        /// </summary>
        /// <value>
        /// The content service.
        /// </value>
        public IContentService ContentService { get; private set; }

        public bool PublishedOnly { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoService"/> class.
        /// </summary>
        /// <param name="contentService">The content service.</param>
        /// <param name="contextName">Name of the context.</param>
        public UmbracoService(IContentService contentService, string contextName = "Default")
            :base(contextName)
        {
            ContentService = contentService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoService"/> class.
        /// </summary>
        /// <param name="contentService">The content service.</param>
        /// <param name="context">The context.</param>
        public UmbracoService(IContentService contentService, Context context)
            : base(context ?? Context.Default)
        {
            ContentService = contentService;
        }

        /// <summary>
        /// Gets the home item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns></returns>
        public T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            var item = ContentService.GetChildren(-1).FirstOrDefault();
            return CreateType(typeof(T), item, isLazy, inferType) as T;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns></returns>
        public T GetItem<T>(int? id, bool isLazy = false, bool inferType = false) where T : class
        {
            if (id == null)
            {
                return null;
            }
            var item = PublishedOnly
                           ? ContentService.GetPublishedVersion(id.Value)
                           : ContentService.GetById(id.Value);

            return CreateType(typeof(T), item, isLazy, inferType) as T;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns></returns>
        public T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T : class
        {
            var item = ContentService.GetById(id);

            if (PublishedOnly)
                item = ContentService.GetPublishedVersion(item.Id);

            return CreateType(typeof(T), item, isLazy, inferType) as T;
        }

        /// <summary>
        /// Saves the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <exception cref="System.NullReferenceException">Can not save class, could not find configuration for {0}.Formatted(typeof(T).FullName)</exception>
        /// <exception cref="MapperException">Could not save class, conent not found</exception>
        public void Save<T>(T target)
        {
            //TODO: should this be a separate context
            //  UmbracoTypeContext context = new UmbracoTypeContext();

            //TODO: ME - this may not work with a proxy
            var config = GlassContext.GetTypeConfiguration<UmbracoTypeConfiguration>(target);

            if (config == null)
                throw new NullReferenceException("Can not save class, could not find configuration for {0}".Formatted(typeof(T).FullName));

            var item = config.ResolveItem(target, ContentService);
            if (item == null)
                throw new MapperException("Could not save class, conent not found");

            WriteToItem(target, item, config);
        }
        
        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="content">The content.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="constructorParameters">Parameters to pass to the constructor of the new class. Must be in the order specified on the consturctor.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Maximum number of constructor parameters is 4</exception>
        public object CreateType(Type type, IContent content, bool isLazy, bool inferType, params object[] constructorParameters)
        {
            if (content == null) return null;


            if (constructorParameters != null && constructorParameters.Length > 4)
                throw new NotSupportedException("Maximum number of constructor parameters is 4");

            var creationContext = new UmbracoTypeCreationContext
                {
                    UmbracoService = this,
                    RequestedType = type,
                    ConstructorParameters = constructorParameters,
                    Content = content,
                    InferType = inferType,
                    IsLazy = isLazy,
                    PublishedOnly =  PublishedOnly
                };
            var obj = InstantiateObject(creationContext);

            return obj;
        }

        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="content">The content to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public T CreateType<T>(IContent content, bool isLazy = false, bool inferType = false) where T : class
        {
            return (T)CreateType(typeof(T), content, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
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
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
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
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
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
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public T CreateType<T, K, L, M, N>(IContent content, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false)
        {
            return (T)CreateType(typeof(T), content, isLazy, inferType, param1, param2, param3, param4);
        }

        /// <summary>
        /// Creates a new Umbraco class.
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the UmbracoClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the UmbracoIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute UmbracoInfoAttribute of type UmbracoInfoType.Name or the fluent equivalent</param>
        /// <returns></returns>
        /// <exception cref="MapperException">
        /// Failed to find configuration for new item type {0}.Formatted(typeof(T).FullName)
        /// or
        /// Failed to find configuration for parent item type {0}.Formatted(typeof(int).FullName)
        /// or
        /// Could not find parent item
        /// or
        /// The type {0} does not have a property with attribute UmbracoInfo(UmbracoInfoType.Name).Formatted(newType.Type.FullName)
        /// or
        /// Failed to create item
        /// </exception>
        public T Create<T, TParent>(TParent parent, T newItem) where T : class 
                                                               where TParent : class
        {
            UmbracoTypeConfiguration newType;

            try
            {
                newType = GlassContext.GetTypeConfiguration<UmbracoTypeConfiguration>(newItem);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for new item type {0}".Formatted(typeof(T).FullName), ex);
            }
            
            UmbracoTypeConfiguration parentType;

            try
            {
                parentType = GlassContext.GetTypeConfiguration<UmbracoTypeConfiguration>(parent);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for parent item type {0}".Formatted(typeof(int).FullName), ex);
            }
            
            var pItem = parentType.ResolveItem(parent, ContentService);

            if (pItem == null)
                throw new MapperException("Could not find parent item");
            
            var nameProperty = newType.Properties.Where(x => x is UmbracoInfoConfiguration)
                .Cast<UmbracoInfoConfiguration>().FirstOrDefault(x => x.Type == UmbracoInfoType.Name);

            if (nameProperty == null)
                throw new MapperException("The type {0} does not have a property with attribute UmbracoInfo(UmbracoInfoType.Name)".Formatted(newType.Type.FullName));
            
            string tempName = Guid.NewGuid().ToString();
            var content = ContentService.CreateContent(tempName, pItem, newType.ContentTypeAlias);

            if (content == null) throw new MapperException("Failed to create item");

            //write new data to the item

            WriteToItem(newItem, content);

            //then read it back

            var typeContext = new UmbracoTypeCreationContext
                {
                    Content = content, 
                    UmbracoService = this
                };

            newType.MapPropertiesToObject(newItem, this, typeContext);

            return newItem;
        }

        /// <summary>
        /// Writes to item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="content">The content.</param>
        /// <param name="config">The config.</param>
        public void WriteToItem<T>(T target, IContent content, UmbracoTypeConfiguration config = null)
        {
            if (config == null)
                config = GlassContext.GetTypeConfiguration<UmbracoTypeConfiguration>(target);

            var savingContext = new UmbracoTypeSavingContext
                {
                    Config = config, 
                    Content = content, 
                    Object = target
                };

            //ME-an item with no versions should be null

            SaveObject(savingContext);
        }

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="MapperException"></exception>
        public void Delete<T>(T item) where T : class
        {
            var type = GlassContext.GetTypeConfiguration <UmbracoTypeConfiguration>(item) as UmbracoTypeConfiguration;
            var umbItem = type.ResolveItem(item, ContentService);

            if (umbItem == null) throw new MapperException("Content not found");

            ContentService.Delete(umbItem);
        }

        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <param name="abstractTypeSavingContext">The abstract type saving context.</param>
        public override void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext)
        {
            ContentService.Save(((UmbracoTypeSavingContext) abstractTypeSavingContext).Content);
            base.SaveObject(abstractTypeSavingContext);
        }

        /// <summary>
        /// Creates the data mapping context.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var umbTypeContext = abstractTypeCreationContext as UmbracoTypeCreationContext;
            return new UmbracoDataMappingContext(obj, umbTypeContext.Content, this, umbTypeContext.PublishedOnly);
        }

        /// <summary>
        /// Used to create the context used by DataMappers to map data from a class
        /// </summary>
        /// <param name="creationContext"></param>
        /// <returns></returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var umbContext = creationContext as UmbracoTypeSavingContext;
            return new UmbracoDataMappingContext(umbContext.Object, umbContext.Content, this, umbContext.PublishedOnly);
        }
    }
}

