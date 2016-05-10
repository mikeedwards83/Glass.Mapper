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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Dynamic;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Version = Sitecore.Data.Version;

namespace Glass.Mapper.Sc
{


    /// <summary>
    /// Class SitecoreService
    /// </summary>
    public class SitecoreService : AbstractService, ISitecoreService
    {
        public Config Config { get; set; }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        public  Database Database { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(Database database, string contextName = "Default")
            :base(contextName)
        {
            Database = database;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(string databaseName, string contextName = "Default")
            : base(contextName)
        {
            Database = Factory.GetDatabase(databaseName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="context">The context.</param>
        public SitecoreService(string databaseName, Context context)
            : base(context ?? Context.Default )
        {
            Database = Factory.GetDatabase(databaseName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="context">The context.</param>
        public SitecoreService(Database database, Context context)
            : base(context ?? Context.Default)
        {
            Database = database;
        }



        public override void Initiate(IDependencyResolver resolver)
        {
            Config = resolver.GetConfig() as Config;
            
            base.Initiate(resolver);
            
            CacheEnabled = Sitecore.Context.Site == null ||
                           (!Sitecore.Context.PageMode.IsPageEditor &&
                            !Sitecore.Context.PageMode.IsPageEditorEditing &&
                            (Sitecore.Context.Site != null && Sitecore.Context.Site.Properties["glassCache"] == "true"));
        }


        #region AddVersion

        /// <summary>
        /// Adds a version of the item
        /// </summary>
        /// <typeparam name="T">The type being added. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="target">The class to add a version to</param>
        /// <returns>``0.</returns>
        /// <exception cref="System.NullReferenceException">Can not add version, could not find configuration for {0}.Formatted(typeof(T).FullName)</exception>
        /// <exception cref="Glass.Mapper.MapperException">Could not add version, item not found</exception>
        public T AddVersion<T>(T target) where T : class
        {
            //TODO: ME - this may not work with a proxy
            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target) ;

            if (config == null)
                throw new NullReferenceException("Can not add version, could not find configuration for {0}".Formatted(typeof(T).FullName));

            var item = config.ResolveItem(target, Database);
            if (item == null)
                throw new MapperException("Could not add version, item not found");

            Item newVersion = item.Versions.AddVersion();

            return CreateType<T>(newVersion);

        }


        #endregion

        #region Create

        /// <summary>
        /// Creates a new Sitecore item.
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the SitecoreClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="TK">The type of the parent item</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the SitecoreIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute SitecoreInfoAttribute of type SitecoreInfoType.Name or the fluent equivalent</param>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <returns>``0.</returns>
        /// <exception cref="Glass.Mapper.MapperException">
        /// Failed to find configuration for new item type {0}.Formatted(typeof(T).FullName)
        /// or
        /// Failed to find configuration for parent item type {0}.Formatted(typeof(K).FullName)
        /// or
        /// Could not find parent item
        /// or
        /// The type {0} does not have a property with attribute SitecoreInfo(SitecoreInfoType.Name).Formatted(newType.Type.FullName)
        /// or
        /// Type {0} does not have a Template ID or Branch ID.Formatted(typeof(T).FullName)
        /// or
        /// Failed to create item
        /// </exception>
        public T Create<T, TK>(TK parent, T newItem, bool updateStatistics = true, bool silent = false)
            where T : class
            where TK : class
        {

            SitecoreTypeConfiguration newType;
            try
            {
              newType = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(newItem, checkBase:false);
               
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for new item type {0}".Formatted(typeof(T).FullName), ex);
            }


            SitecoreTypeConfiguration parentType;
            try
            {
                parentType = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(parent);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for parent item type {0}".Formatted(typeof(TK).FullName), ex);
            }

            Item parentItem = parentType.ResolveItem(parent, Database);

            
            if (parentItem == null)
                throw new MapperException("Could not find parent item");


            var nameProperty = newType.Properties.Where(x => x is SitecoreInfoConfiguration)
                .Cast<SitecoreInfoConfiguration>().FirstOrDefault(x => x.Type == SitecoreInfoType.Name);

            if (nameProperty == null)
                throw new MapperException("The type {0} does not have a property with attribute SitecoreInfo(SitecoreInfoType.Name)".Formatted(newType.Type.FullName));

            string name = string.Empty;

            try
            {
                name = nameProperty.PropertyInfo.GetValue(newItem, null).ToString();
            }
            catch
            {
                throw new MapperException("Failed to get item name");
            }

            if (name.IsNullOrEmpty())
                throw new MapperException("New class has no name");
               
            ID templateId = newType.TemplateId;
            ID branchId = newType.BranchId;
            ID itemId = newType.GetId(newItem);
            Language language = newType.GetLanguage(newItem);

            //check that parent item language is equal to new item language, if not change parent to other language
            if (language != null && parentItem.Language != language)
            {
                parentItem = Database.GetItem(parentItem.ID, language);
            }

            Item item;
            if (!ID.IsNullOrEmpty(itemId) && ID.IsNullOrEmpty(branchId) && !ID.IsNullOrEmpty(templateId))
            {
                item = ItemManager.AddFromTemplate(name, templateId, parentItem, itemId);
            }
            else if (!ID.IsNullOrEmpty(branchId))
            {
                item = parentItem.Add(name, new BranchId(branchId));
            }
            else if (!ID.IsNullOrEmpty(templateId))
            {
                item = parentItem.Add(name, new TemplateID(templateId));
            }
            else
            {
                throw new MapperException("Type {0} does not have a Template ID or Branch ID".Formatted(typeof(T).FullName));
            }

            if (item == null) throw new MapperException("Failed to create item");

            //write new data to the item

            WriteToItem(newItem, item, updateStatistics, silent);

            //then read it back

            SitecoreTypeCreationContext typeContext = new SitecoreTypeCreationContext();
            typeContext.Item = item;
            typeContext.SitecoreService = this;

            newType.MapPropertiesToObject(newItem, this, typeContext);

            return newItem;
        }

        public T Create<T, TK>(TK parent, string newName, Language language = null, bool updateStatistics = true, bool silent = false)
            where T : class
            where TK : class
        {

            SitecoreTypeConfiguration newType;
            try
            {
                newType = GlassContext.GetTypeConfigurationFromType<SitecoreTypeConfiguration>(typeof(T)) ;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for new item type {0}".Formatted(typeof(T).FullName), ex);
            }


            SitecoreTypeConfiguration parentType;
            try
            {
                parentType = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(parent);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for parent item type {0}".Formatted(typeof(TK).FullName), ex);
            }

            Item pItem = parentType.ResolveItem(parent, Database);


            if (pItem == null)
                throw new MapperException("Could not find parent item");

            
            if (newName.IsNullOrEmpty())
                throw new MapperException("New class has no name");

            ID templateId = newType.TemplateId;
            ID branchId = newType.BranchId;
            

            //check that parent item language is equal to new item language, if not change parent to other language
            if (language != null && pItem.Language != language)
            {
                pItem = Database.GetItem(pItem.ID, language);
            }

            Item item;




            if (!ID.IsNullOrEmpty(branchId))
            {
                item = pItem.Add(newName, new BranchId(branchId));
            }
            else if (!ID.IsNullOrEmpty(templateId))
            {
                item = pItem.Add(newName, new TemplateID(templateId));
            }
            else
            {
                throw new MapperException("Type {0} does not have a Template ID or Branch ID".Formatted(typeof(T).FullName));
            }

            if (item == null) throw new MapperException("Failed to create item");

            
            return this.CreateType<T>(item, false, false);
        }



        #endregion

        #region CreateType

        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T>(Item item, bool isLazy = false, bool inferType = false) where T : class
        {
            return CreateType<T>(item, isLazy, inferType, null);
        }

        /// <summary>
        /// Casts a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T Cast<T>(Item item, bool isLazy = false, bool inferType = false) where T : class
        {
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, TK>(Item item, TK param1, bool isLazy = false, bool inferType = false)
        {
            return CreateType<T>(item, isLazy, inferType, param1);

        }

        /// <summary>
        /// Creates a class from the specified item with two constructor parameters
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, TK, TL>(Item item, TK param1, TL param2, bool isLazy = false, bool inferType = false)
        {
            return CreateType<T>(item, isLazy, inferType, param1, param2);
        }

        /// <summary>
        /// Creates a class from the specified item with three constructor parameters
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, TK, TL, TM>(Item item, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false)
        {
            return CreateType<T>(item, isLazy, inferType, param1, param2, param3);
        }

        /// <summary>
        /// Creates a class from the specified item with four constructor parameters
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, TK, TL, TM, TN>(Item item, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false)
        {
            return CreateType<T>(item, isLazy, inferType, param1, param2, param3, param4);
        }

        /// <summary>
        /// Creates a class from the specified item with fifth constructor parameters
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <typeparam name="TO">The type of the fifth constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="param5">The value of the fifth parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T, TK, TL, TM, TN, TO>(Item item, TK param1, TL param2, TM param3, TN param4, TO param5, bool isLazy = false, bool inferType = false)
        {
            return CreateType<T>(item, isLazy, inferType, param1, param2, param3, param4, param5);
        }

        /// <summary>
        /// Creates a class from the specified item with constructor parameters
        /// </summary>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="constructorParameters">The constructor parameters</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        public T CreateType<T>(Item item, bool isLazy = false, bool inferType = false, params object[] constructorParameters)
        {
            return (T)CreateType(typeof(T), item, isLazy, inferType, null, constructorParameters);
        }
        
        public object CreateType(Type type, Item item, bool isLazy, bool inferType, Dictionary<string, object> parameters, params object[] constructorParameters)
        {
            if (item == null || (item.Versions.Count == 0 && Utilities.DoVersionCheck(Config))) return null;


            if (constructorParameters != null && constructorParameters.Length > 10)
                throw new NotSupportedException("Maximum number of constructor parameters is 10");

            SitecoreTypeCreationContext creationContext = new SitecoreTypeCreationContext();
            creationContext.SitecoreService = this;
            creationContext.RequestedType = type;
            creationContext.ConstructorParameters = constructorParameters;
            creationContext.Item = item;
            creationContext.InferType = inferType;
            creationContext.IsLazy = isLazy;
            creationContext.Parameters = parameters ?? new Dictionary<string, object>();

            var obj = InstantiateObject(creationContext);

            return obj;
        }

        #endregion
        
        #region CreateTypes

        /// <summary>
        /// Create a collection of classes from the specified type
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for each class</param>
        /// <param name="inferType">Infer the type to be loaded from the item template</param>
        /// <param name="type">The type to return</param>
        /// <param name="getItems">A function that returns the list of items to load</param>
        /// <returns>An enumerable of the items as the specified type</returns>
        public IEnumerable CreateTypes(Type type, Func<IEnumerable<Item>> getItems, bool isLazy = false, bool inferType = false)
        {
            return Utilities.CreateGenericType(typeof(LazyItemEnumerable<>), new[] { type }, getItems, isLazy, inferType, this) as IEnumerable;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete an item from Sitecore
        /// </summary>
        /// <typeparam name="T">The type being deleted. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to delete</param>
        /// <exception cref="Glass.Mapper.MapperException">Item not found</exception>
        public void Delete<T>(T item) where T : class
        {

            var type = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(item);

            Item scItem = type.ResolveItem(item, Database);

            if (scItem == null)
                throw new MapperException("Item not found");

            scItem.Delete();
        }

        #endregion

        #region Dynamics

        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="id">The ID of the item to return</param>
        /// <returns>dynamic.</returns>
        public dynamic GetDynamicItem(Guid id)
        {
            return GetDynamicItem(Database.GetItem(new ID(id)));
        }

        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="path">The path of the item to return</param>
        /// <returns>dynamic.</returns>
        public dynamic GetDynamicItem(string path)
        {
            return GetDynamicItem(Database.GetItem(path));
        }

        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="item">The item to convert</param>
        /// <returns>dynamic.</returns>
        public dynamic GetDynamicItem(Item item)
        {
            if (item == null) return null;
            return new DynamicItem(item);
        }


        #endregion

        #region GetItem - Path

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>``0.</returns>
        public T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class
        {
            var item = Database.GetItem(path);
            return CreateType(typeof(T), item, isLazy, inferType, null) as T;
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        public T GetItem<T, TK>(string path, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path);
            return CreateType<T, TK>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        public T GetItem<T, TK, TL>(string path, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path);
            return CreateType<T, TK, TL>(item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM>(string path, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path);
            return CreateType<T, TK, TL, TM>(item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM, TN>(string path, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path);
            return CreateType<T, TK, TL, TM, TN>(item, param1, param2, param3, param4, isLazy, inferType);
        }

        #endregion

        #region GetItem - Path, Language

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T>(string path, Language language, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK>(string path, Language language, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, TK>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL>(string path, Language language, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, TK, TL>(item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM>(string path, Language language, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, TK, TL, TM>(item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM, TN>(string path, Language language, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language);
            return CreateType<T, TK, TL, TM, TN>(item, param1, param2, param3, param4, isLazy, inferType);
        }



        #endregion

        #region GetItem - Path, Language, Version

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T>(string path, Language language, Version version, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language, version);
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK>(string path, Language language, Version version, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language, version);
            return CreateType<T, TK>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL>(string path, Language language, Version version, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language, version);
            return CreateType<T, TK, TL>(item, param1, param2, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM>(string path, Language language, Version version, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language, version);
            return CreateType<T, TK, TL, TM>(item, param1, param2, param3, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM, TN>(string path, Language language, Version version, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(path, language, version);
            return CreateType<T, TK, TL, TM, TN>(item, param1, param2, param3, param4, isLazy, inferType);
        }

        #endregion

        #region GetItem - Guid

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>``0.</returns>
        public T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T : class
        {
            var item = Database.GetItem(new ID(id));
            return CreateType(typeof(T), item, isLazy, inferType, null) as T;
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK>(Guid id, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id));
            return CreateType<T, TK>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL>(Guid id, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id));
            return CreateType<T, TK, TL>(item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM>(Guid id, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id));
            return CreateType<T, TK, TL, TM>(item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM, TN>(Guid id, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id));
            return CreateType<T, TK, TL, TM, TN>(item, param1, param2, param3, param4, isLazy, inferType);
        }



        #endregion

        #region GetItem - Guid, Language

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T>(Guid id, Language language, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK>(Guid id, Language language, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, TK>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL>(Guid id, Language language, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, TK, TL>(item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM>(Guid id, Language language, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, TK, TL, TM>(item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM, TN>(Guid id, Language language, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language);
            return CreateType<T, TK, TL, TM, TN>(item, param1, param2, param3, param4, isLazy, inferType);
        }

        #endregion

        #region GetItem - Guid, Language, Version

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T>(Guid id, Language language, Version version, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK>(Guid id, Language language, Version version, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, TK>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL>(Guid id, Language language, Version version, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, TK, TL>(item, param1, param2, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM>(Guid id, Language language, Version version, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, TK, TL, TM>(item, param1, param2, param3, isLazy, inferType);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetItem<T, TK, TL, TM, TN>(Guid id, Language language, Version version, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.GetItem(new ID(id), language, version);
            return CreateType<T, TK, TL, TM, TN>(item, param1, param2, param3, param4, isLazy, inferType);
        }
        
        #endregion


        #region  GetItemWithInterfaces

        public T GetItemWithInterfaces<T, TK, TL, TM, TN>(Guid id, Language language = null, Version version = null, bool isLazy = false,
          bool inferType = false) where T : class where TK: class where TL:class where TM: class where TN: class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] = 
                new[] {typeof (TK), typeof (TL), typeof (TM), typeof (TN)};

            Item item = Database.GetItem(new ID(id), language, version);
            return (T) CreateType(
                typeof (T), 
                item,
                isLazy,
                inferType,
                parameters);
        }

        public T GetItemWithInterfaces<T, TK, TL, TM>(Guid id, Language language = null, Version version = null, bool isLazy = false,
                                                  bool inferType = false) where T : class where TK : class where TL : class where TM : class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] =
                new[] { typeof(TK), typeof(TL), typeof(TM) };

            Item item = Database.GetItem(new ID(id), language, version);
            return (T)CreateType(
                 typeof(T),// typeof(TK), typeof(TL), typeof(TM)},
                item,
                isLazy,
                inferType,
                parameters);
        }

        public T GetItemWithInterfaces<T, TK, TL>(Guid id, Language language = null, Version version = null, bool isLazy = false,
                                              bool inferType = false) where T : class where TK : class where TL : class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] =
                new[] { typeof(TK), typeof(TL) };

            Item item = Database.GetItem(new ID(id), language, version);
            return (T)CreateType(
                 typeof(T),// typeof(TK), typeof(TL) },
                item,
                isLazy,
                inferType,
                parameters);
        }

        public T GetItemWithInterfaces<T, TK>(Guid id, Language language = null, Version version = null, bool isLazy = false,
                                          bool inferType = false) where T : class where TK : class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] =
                new[] { typeof(TK) };

            Item item = Database.GetItem(new ID(id), language, version);
            return (T)CreateType(
                typeof(T),// typeof(TK) },
                item,
                isLazy,
                inferType,
                parameters);
        }

        public T GetItemWithInterfaces<T, TK, TL, TM, TN>(string path, Language language = null, Version version = null, bool isLazy = false,
  bool inferType = false)
            where T : class
            where TK : class
            where TL : class
            where TM : class
            where TN : class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] =
                new[] { typeof(TK), typeof(TL), typeof(TM), typeof(TN) };

            Item item = Database.GetItem(path, language, version);
            return (T)CreateType(
                typeof(T), // typeof(TK), typeof(TL), typeof(TM), typeof(TN) },
                item,
                isLazy,
                inferType,
                parameters);
        }

        public T GetItemWithInterfaces<T, TK, TL, TM>(string path, Language language = null, Version version = null, bool isLazy = false,
                                                  bool inferType = false)
            where T : class
            where TK : class
            where TL : class
            where TM : class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] =
                new[] { typeof(TK), typeof(TL), typeof(TM) };

            Item item = Database.GetItem(path, language, version);
            return (T)CreateType(
                 typeof(T),// typeof(TK), typeof(TL), typeof(TM) },
                item,
                isLazy,
                inferType,
                parameters);
        }

        public T GetItemWithInterfaces<T, TK, TL>(string path, Language language = null, Version version = null, bool isLazy = false,
                                              bool inferType = false)
            where T : class
            where TK : class
            where TL : class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] =
                new[] { typeof(TK), typeof(TL) };

            Item item = Database.GetItem(path, language, version);
            return (T)CreateType(
                 typeof(T), //typeof(TK), typeof(TL) },
                item,
                isLazy,
                inferType,
                parameters);
        }

        public T GetItemWithInterfaces<T, TK>(string path, Language language = null, Version version = null, bool isLazy = false,
                                          bool inferType = false)
            where T : class
            where TK : class
        {
            language = language ?? Language.Current;
            version = version ?? Version.Latest;

            var parameters = new Dictionary<string, object>();
            parameters[MultiInterfaceResolverTask.MultiInterfaceTypesKey] =
                new[] { typeof(TK)};

            Item item = Database.GetItem(path, language, version);
            return (T)CreateType(
                 typeof(T), //typeof(TK) },
                item,
                isLazy,
                inferType,
                parameters);
        }

        #endregion

        #region Move

        /// <summary>
        /// Moves the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="newParent">The new parent.</param>
        public void Move<T, TK>(T item, TK newParent)
        {
            var itemType = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(item);
            var parentType = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(newParent);

            Item scItem = itemType.ResolveItem(item, Database);
            Item scNewParent = parentType.ResolveItem(newParent, Database);

            scItem.MoveTo(scNewParent);
        }


        #endregion

        #region  Query

        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        public IEnumerable<T> Query<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            return CreateTypes( typeof(T), () => { return Database.SelectItems(query); }, isLazy, inferType) as IEnumerable<T>;
        }


        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="language"></param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        public IEnumerable<T> Query<T>(string query, Language language, bool isLazy = false, bool inferType = false) where T : class
        {
            var items = Database.SelectItems(query);
            return items
                .Select(x => GetItem<T>(x.ID.Guid, language, isLazy, inferType))
                .Where(x => x != null);
        }

        #endregion

        #region QuerySingle

        /// <summary>
        /// Query Sitecore for a single item.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore item as the specified type</returns>
        public T QuerySingle<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = Database.SelectSingleItem(query);
            return CreateType<T>(item, isLazy, inferType);
        }

        #endregion

        #region Save


        /// <summary>
        /// Saves the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <exception cref="System.NullReferenceException">Can not save class, could not find configuration for {0}.Formatted(typeof(T).FullName)</exception>
        /// <exception cref="Glass.Mapper.MapperException">Could not save class, item not found</exception>
        public void Save<T>(T target, bool updateStatistics = true, bool silent = false)
        {
            //TODO: should this be a separate context
            //  SitecoreTypeContext context = new SitecoreTypeContext();

            //TODO: ME - this may not work with a proxy
            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target);

            if (config == null)
                throw new NullReferenceException("Can not save class, could not find configuration for {0}".Formatted(typeof(T).FullName));

            var item = config.ResolveItem(target, Database);
            if (item == null)
                throw new MapperException("Could not save class, item not found");

            WriteToItem(target, item, updateStatistics, silent);

        }

        #endregion

        #region WriteToItem

        /// <summary>
        /// Writes to item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <param name="target">The target.</param>
        /// <param name="item">The item.</param>
        public void WriteToItem<T>(T target, Item item, bool updateStatistics = true, bool silent = false)
        {
            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target);

            SitecoreTypeSavingContext savingContext = new SitecoreTypeSavingContext();
            savingContext.Config = config;

            //ME-an item with no versions should be null

            savingContext.Item = item;
            savingContext.Object = target;

            item.Editing.BeginEdit();

            SaveObject(savingContext);

            item.Editing.EndEdit(updateStatistics, silent);
        }


        #endregion

        #region Map

        public void Map<T>(T target)
        {
            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target);

            if(config == null)
                throw new MapperException("No configuration for type {0}. Load configuration using Attribute or Fluent configuration.".Formatted(typeof(T).Name));

            var item = config.ResolveItem(target, Database);

            if (item == null)
                return;

            SitecoreTypeCreationContext creationContext = new SitecoreTypeCreationContext();
            creationContext.SitecoreService = this;
            creationContext.RequestedType = typeof (T);
            creationContext.ConstructorParameters = new object[0];
            creationContext.Item = item;
            creationContext.InferType = false;
            creationContext.IsLazy = false;
            creationContext.Parameters = new Dictionary<string, object>();

            config.MapPropertiesToObject(target, this,creationContext);
        }

        #endregion

        #region ResolveItem

        public Item ResolveItem(object target)
        {
            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target);
            
            if (config == null)
            {
                return null;
            }

            var item = config.ResolveItem(target, Database);

            return item;

        }

        #endregion

        /// <summary>
        /// Creates the data mapping context.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>AbstractDataMappingContext.</returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var scTypeContext =  abstractTypeCreationContext as SitecoreTypeCreationContext;

           System.Diagnostics.Debug.Assert(scTypeContext != null, "Creation context is null");

            return new SitecoreDataMappingContext(obj, scTypeContext.Item, this);
        }

        /// <summary>
        /// Used to create the context used by DataMappers to map data from a class
        /// </summary>
        /// <param name="creationContext">The Saving Context</param>
        /// <returns>AbstractDataMappingContext.</returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var scContext = creationContext as SitecoreTypeSavingContext;

            System.Diagnostics.Debug.Assert(scContext != null, "Saving context is null");

            return new SitecoreDataMappingContext(scContext.Object, scContext.Item, this);
        }



    } 
}




