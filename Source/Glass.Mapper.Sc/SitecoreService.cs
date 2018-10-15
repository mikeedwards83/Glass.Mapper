using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Builders;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands.Masters;
using IDependencyResolver = Glass.Mapper.IoC.IDependencyResolver;

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
        public Database Database { get; private set; }

        public LazyLoadingHelper LazyLoadingHelper { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="context">The context.</param>
        public SitecoreService(Database database, Context context)
            : base(context)
        {
            Database = database;
            var di = GlassContext.DependencyResolver as Sc.IoC.IDependencyResolver;
            LazyLoadingHelper = di.LazyLoadingHelper;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(Database database)
            : this(database, Context.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(string databaseName, string contextName = "Default")
            : this(Factory.GetDatabase(databaseName), Context.Contexts[contextName])
        {

        }
        public override void Initiate(IDependencyResolver resolver)
        {
            Config = resolver.GetConfig() as Config;

            base.Initiate(resolver);

            CacheEnabled = Sitecore.Context.Site == null ||
                           (!Utilities.IsPageEditor &&
                            !Utilities.IsPageEditorEditing &&
                            (Sitecore.Context.Site != null && Sitecore.Context.Site.Properties["glassCache"] != "false"));
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
            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target);

            if (config == null)
                throw new NullReferenceException("Can not add version, could not find configuration for {0}".Formatted(typeof(T).FullName));

            var item = config.ResolveItem(target, Database);
            if (item == null)
                throw new MapperException("Could not add version, item not found");

            Item newVersion = item.Versions.AddVersion();

            return this.GetItem<T>(new GetItemByItemOptions() { Item = newVersion });
        }



        #endregion


        #region Delete

        /// <summary>
        /// Delete an item from Sitecore
        /// </summary>
        /// <typeparam name="T">The type being deleted. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to delete</param>
        /// <exception cref="Glass.Mapper.MapperException">Item not found</exception>
        public void DeleteItem(DeleteByModelOptions options)
        {
            options.Validate();

            var type = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.Model);

            Item scItem = type.ResolveItem(options.Model, Database);

            if (scItem == null)
                throw new MapperException("Item not found");

            scItem.Delete();
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
        public void MoveItem(MoveByModelOptions options)
        {
            options.Validate();

            var itemType = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.Model);
            var parentType = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.NewParent);

            Item scItem = itemType.ResolveItem(options.Model, Database);
            Item scNewParent = parentType.ResolveItem(options.NewParent, Database);

            scItem.MoveTo(scNewParent);
        }


        #endregion


        #region Save


        /// <summary>
        /// Saves the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">Options for how the model will be saved</param>
        public void SaveItem(SaveOptions options)
        {
            Assert.IsNotNull(options, "options must no be  null");

            //TODO: ME - this may not work with a proxy
            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.Model);

            if (config == null)
                throw new NullReferenceException("Can not save class, could not find configuration for {0}".Formatted(options.Type.FullName));

            var item = config.ResolveItem(options.Model, Database);
            if (item == null)
                throw new MapperException("Could not save class, item not found");

            var writeToItemOptions = new WriteToItemOptions();
            writeToItemOptions.Copy(options);
            writeToItemOptions.Item = item;
            writeToItemOptions.Model = options.Model;

            WriteToItem(writeToItemOptions);
        }

        #endregion

        #region WriteToItem

        /// <summary>
        /// Writes to item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">Options for how the model will be saved</param>
        public void WriteToItem(WriteToItemOptions options)
        {

            Assert.IsNotNull(options, "options must no be  null");

            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.Model);

            SitecoreTypeSavingContext savingContext = new SitecoreTypeSavingContext();
            savingContext.Config = config;

            //ME-an item with no versions should be null

            savingContext.Item = options.Item;
            savingContext.Object = options.Model;

            options.Item.Editing.BeginEdit();

            SaveObject(savingContext);

            options.Item.Editing.EndEdit(options.UpdateStatistics, options.Silent);
        }


        #endregion

        #region Map

        public void Populate<T>(T target) where T : class
        {
            Populate(target, new GetOptions());
        }

        public void Populate<T>(T target, GetOptions options) where T : class
        {

            Assert.IsNotNull(options, "options must no be  null");

            var config = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target);

            if (config == null)
                throw new MapperException("No configuration for type {0}. Load configuration using Attribute or Fluent configuration.".Formatted(typeof(T).Name));

            var item = config.ResolveItem(target, Database);

            if (item == null)
                return;

            SitecoreTypeCreationContext creationContext = new SitecoreTypeCreationContext();
            creationContext.SitecoreService = this;
            creationContext.Options = options;
            creationContext.Item = item;
            creationContext.Parameters = new Dictionary<string, object>();

            config.MapPropertiesToObject(target, this, creationContext);
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
        /// Returns a collection of items as the requested type.
        /// </summary>
        /// <typeparam name="T">The type to map items to</typeparam>
        /// <param name="options">Options for how the items will be mapped</param>
        /// <returns>The collection of items mapped to the requested type</returns>
        public IEnumerable<T> GetItems<T>(GetItemsOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must no be  null");

            options.Type = typeof(T);
            var results = GetItems(options) as IEnumerable<T>;
            return results;
        }

        /// <summary>
        /// Returns a collection of items as the requested type.
        /// </summary>
        /// <param name="options">Options for how the items will be mapped</param>
        /// <returns>The collection of items mapped to the requested type</returns>
        public IEnumerable<object> GetItems(GetItemsOptions options)
        {
            Assert.IsNotNull(options, "options must no be  null");

            options.Validate();
            return Utilities.CreateGenericType(typeof(LazyItemEnumerable<>), new[] { options.Type }, options, this, LazyLoadingHelper) as IEnumerable<object>;
        }

        /// <summary>
        /// Returns a an item as the requested type.
        /// </summary>
        /// <typeparam name="T">The type to map the item to</typeparam>
        /// <param name="options">Options for how the model will be mapped</param>
        /// <returns>The item mapped to the requested type</returns>
        public T GetItem<T>(GetItemOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must no be  null");

            options.Type = typeof(T);
            return (T)GetItem(options);
        }


        /// <summary>
        /// Returns a an item as the requested type.
        /// </summary>
        /// <param name="options">Options for how the model will be mapped</param>
        /// <returns>The item mapped to the requested type</returns>
        public object GetItem(GetItemOptions options)
        {
            Assert.IsNotNull(options, "options must no be  null");

            options.Validate();
            Item item = options.GetItem(Database);
            return RunCreateType(item, options, null);
        }

        public object RunCreateType(Item item, GetOptions options, Dictionary<string, object> parameters)
        {
            SitecoreTypeCreationContext creationContext = new SitecoreTypeCreationContext();
            creationContext.SitecoreService = this;
            creationContext.Options = options;
            creationContext.Item = item;
            creationContext.Parameters = parameters ?? new Dictionary<string, object>();

            var obj = InstantiateObject(creationContext);

            return obj;
        }


        /// <summary>
        /// Create an item in the Sitecore database
        /// </summary>
        /// <typeparam name="T">The type of model to return the created item as.</typeparam>
        /// <param name="options">Options for how the model will be created</param>
        /// <returns>The new item mapped to the requested type</returns>
        public object CreateItem(CreateOptions options)
        {
            Assert.IsNotNull(options, "options must no be  null");
            options.Validate();

            var byModel = options as CreateByModelOptions;

            if (byModel != null)
            {
                return CreateFromNewModel(byModel);
            }

            var byName = options as CreateByNameOptions;
            if (byName != null)
            {
                return CreateFromName(byName);
            }

            throw new MapperException("Error calling item create");
        }


        /// <summary>
        /// Create an item in the Sitecore database
        /// </summary>
        /// <typeparam name="T">The type of model to return the created item as.</typeparam>
        /// <param name="options">Options for how the model will be created</param>
        /// <returns>The new item mapped to the requested type</returns>
        public T CreateItem<T>(CreateOptions options)
            where T : class
        {
            Assert.IsNotNull(options, "options must no be  null");
            options.Type = typeof(T);
            return CreateItem(options) as T;
        }

        /// <summary>
        /// Create an item in the Sitecore database by specifying the item name 
        /// </summary>
        /// <typeparam name="T">The type of model to return the created item as.</typeparam>
        /// <param name="options">Options for how the model will be created</param>
        /// <returns>The new item mapped to the requested type</returns>
        protected object CreateFromName(CreateByNameOptions options)
        {
            Assert.IsNotNull(options, "options must no be  null");

            SitecoreTypeConfiguration newItemConfig;

            try
            {
                newItemConfig = GlassContext.TypeConfigurations[options.Type] as SitecoreTypeConfiguration;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for new item type {0}".Formatted(options.Type.FullName), ex);
            }


            SitecoreTypeConfiguration parentItemConfig;
            try
            {
                parentItemConfig = GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.Parent, checkBase: true);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to find configuration for parent item type {0}".Formatted(options.ParentType.FullName), ex);
            }


            Item parentItem = parentItemConfig.ResolveItem(options.Parent, Database);

            if (parentItem == null)
                throw new MapperException("Could not find parent item");


            ID templateId = newItemConfig.TemplateId;
            ID branchId = newItemConfig.BranchId;

            //check that parent item language is equal to new item language, if not change parent to other language
            if (options.Language != null && parentItem.Language != options.Language)
            {
                parentItem = Database.GetItem(parentItem.ID, options.Language);
            }

            Item item;

            if (!ID.IsNullOrEmpty(branchId))
            {
                item = parentItem.Add(options.Name, new BranchId(branchId));
            }
            else if (!ID.IsNullOrEmpty(templateId))
            {
                item = parentItem.Add(options.Name, new TemplateID(templateId));
            }
            else
            {
                throw new MapperException("Type {0} does not have a Template ID or Branch ID".Formatted(options.Type.FullName));
            }

            if (item == null) throw new MapperException("Failed to create item");


            return this.GetItem(new GetItemByItemOptions() { Item = item, Type = options.Type });

        }

        /// <summary>
        /// Create an item in the Sitecore database by specifying a model
        /// </summary>
        /// <typeparam name="T">The type of model to return the created item as.</typeparam>
        /// <param name="options">Options for how the model will be created</param>
        /// <returns>The new item mapped to the requested type</returns>
        protected object CreateFromNewModel(CreateByModelOptions options)
        {
            Assert.IsNotNull(options, "options must no be  null");

            SitecoreTypeConfiguration newItemConfig;
            try
            {
                newItemConfig =
                    GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.Model, checkBase: true) as
                        SitecoreTypeConfiguration;
            }
            catch (Exception ex)
            {
                throw new MapperException(
                    "Failed to find configuration for new item type {0}".Formatted(options.Type.FullName), ex);
            }


            SitecoreTypeConfiguration parentItemConfig;
            try
            {
                parentItemConfig =
                    GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(options.Parent, checkBase: true);
            }
            catch (Exception ex)
            {
                throw new MapperException(
                    "Failed to find configuration for parent item type {0}".Formatted(options.ParentType.FullName), ex);
            }

            Item parentItem = parentItemConfig.ResolveItem(options.Parent, Database);

            if (parentItem == null)
                throw new MapperException("Could not find parent item");


            var name = newItemConfig.GetName(options.Model);

            if (name.IsNullOrEmpty())
                throw new MapperException("New class has no name");

            ID templateId = newItemConfig.TemplateId;
            ID branchId = newItemConfig.BranchId;
            ID itemId = newItemConfig.GetId(options.Model);
            Language language = newItemConfig.GetLanguage(options.Model);

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
                throw new MapperException(
                    "Type {0} does not have a Template ID or Branch ID".Formatted(options.Type.FullName));
            }

            if (item == null) throw new MapperException("Failed to create item");

            //write new data to the item
            var writeToItemOptions = new WriteToItemOptions();
            writeToItemOptions.Copy(options);
            writeToItemOptions.Item = item;
            writeToItemOptions.Model = options.Model;

            WriteToItem(writeToItemOptions);

            //then read it back

            SitecoreTypeCreationContext typeContext = new SitecoreTypeCreationContext();
            typeContext.Item = item;
            typeContext.SitecoreService = this;
            typeContext.Options = new GetItemOptions()
            {
                Type = options.Type
            };

            newItemConfig.MapPropertiesToObject(options.Model, this, typeContext);

            return options.Model;
        }

        #region LegacyMethods

        /// <summary>
        /// This maps the legazy isLazy flag to the new enum.
        /// </summary>
        /// <param name="isLazy"></param>
        /// <returns></returns>
        public LazyLoading GetLazyLoading(bool isLazy)
        {
            return isLazy ? LazyLoading.Enabled : LazyLoading.OnlyReferenced;
        }

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
        [Obsolete("Use this.CreateItem<T>()")]
        public T Create<T, TK>(TK parent, T newItem, bool updateStatistics = true, bool silent = false)
            where T : class
            where TK : class
        {

            var options = new CreateByModelOptions()
            {
                Type = typeof(T),
                Model = newItem,
                Parent = parent,
                UpdateStatistics = updateStatistics,
                Silent = silent
            };

            return this.CreateItem<T>(options);
        }

        /// <summary>
        /// Creates a new Sitecore item.
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the SitecoreClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="TK">The type of the parent item</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the SitecoreIdAttribute or fluent equivalent</param>
        /// <param name="newName">The name of the new item</param>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <returns>``0.</returns>
        [Obsolete("Use this.CreateItem<T, TK>()")]
        public T Create<T, TK>(TK parent, string newName, Language language = null, bool updateStatistics = true,
            bool silent = false)
            where T : class
            where TK : class
        {

            var options = new CreateByNameOptions()
            {
                Type = typeof(T),
                Name = newName,
                Parent = parent,
                UpdateStatistics = updateStatistics,
                Silent = silent,
                Language = language
            };

            return this.CreateItem<T>(options);
        }

        #endregion

        #region  CreateType

        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="constructorParameters">Parameters to pass to the constructor of the new class. Must be in the order specified on the consturctor.</param>
        /// <returns>System.Object.</returns>
        [Obsolete("Use this.GetItem()")]
        public object CreateType(Type type, Item item, bool isLazy, bool inferType, params object[] constructorParameters)
        {
            var options = new GetItemByItemOptions
            {
                Type = type,
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = constructorParameters.Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),

            };
            return this.GetItem(options);
        }

        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("Use this.GetItem<T>()")]
        public T CreateType<T>(Item item, bool isLazy = false,
            bool inferType = false) where T : class
        {

            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
            };
            return this.GetItem<T>(options);

        }

        /// <summary>
        /// Casts a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("Use this.GetItem<T>()")]
        public T Cast<T>(Item item, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
            };
            return this.GetItem<T>(options);
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
        [Obsolete("Use this.GetItem<T, TK>()")]
        public T CreateType<T, TK>(Item item, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
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
        [Obsolete("Use this.GetItem<T, TK, TL>()")]
        public T CreateType<T, TK, TL>(Item item, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }
        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
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
        [Obsolete("Use this.GetItem<T, TK, TL, TM>()")]
        public T CreateType<T, TK, TL, TM>(Item item, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2, param3 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        [Obsolete("Use this.GetItem<T, TK, TL, TM, TN>()")]
        public T CreateType<T, TK, TL, TM, TN>(Item item, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2, param3, param4 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }
        /// <summary>
        /// Creates a class from the specified item with five constructor parameters
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
        [Obsolete("Use this.GetItem<T, TK, TL, TM, TN, TO>()")]
        public T CreateType<T, TK, TL, TM, TN, TO>(Item item, TK param1,
            TL param2, TM param3, TN param4, TO param5, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2, param3, param4, param5 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }

        /// <summary>
        /// Creates a class from the specified item with constructor parameters
        /// </summary>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="constructorParameters">The constructor parameters - maximum 10</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("Use this.GetItem<T>()")]
        public T CreateType<T>(Item item, bool isLazy = false, bool inferType = false, params object[] constructorParameters) where T : class
        {
            var options = new GetItemByItemOptions
            {
                Item = item,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = constructorParameters.Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        [Obsolete("Use this.GetItems()")]
        public IEnumerable CreateTypes(Type type, Func<IEnumerable<Item>> getItems, bool isLazy = false,
            bool inferType = false)
        {

            var options = new GetItemsByFuncOptions()
            {
                ItemsFunc = (Database) => getItems(),
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Type = type

            };
            return this.GetItems(options);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete an item from Sitecore
        /// </summary>
        /// <typeparam name="T">The type being deleted. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to delete</param>
        [Obsolete("Use this.DeleteItem<T>()")]
        public void Delete<T>(T item) where T : class
        {
            var options = new DeleteByModelOptions()
            {
                Model = item
            };
            this.DeleteItem(options);
        }

        #endregion

        #region GetItem - Path


        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        public T GetItem<T>(string path) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T>(string path, bool isLazy) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),

                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T>(string path, bool isLazy, bool inferType) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TL"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL>(string path, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TL"></typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="param3">The param3.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM>(string path, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2, param3 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TL"></typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <typeparam name="TN"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="param3">The param3.</param>
        /// <param name="param4">The param4.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM, TN>(string path, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2, param3, param4 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK>(string path, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T>(string path, Language language, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK>(string path, Language language, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                ConstructorParameters = (new object[] { param1 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL>(string path, Language language, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                ConstructorParameters = (new object[] { param1, param2 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM>(string path, Language language, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                ConstructorParameters = (new object[] { param1, param2, param3 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM, TN>(string path,
            Language language, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false)
            where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                ConstructorParameters = (new object[] { param1, param2, param3, param4 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T>(string path, Language language, Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK>(string path, Language language, Sitecore.Data.Version version, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL>(string path, Language language, Sitecore.Data.Version version, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1, param2 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM>(string path, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1, param2, param3 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM, TN>(string path, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByPathOptions
            {
                Path = path,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1, param2, param3, param4 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }




        #endregion

        #region GetItem - Guid

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        public T GetItem<T>(Guid id) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK>(Guid id, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL>(Guid id, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
        }
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM>(Guid id, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2, param3 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM, TN>(Guid id, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                ConstructorParameters = (new object[] { param1, param2, param3, param4 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T>(Guid id, Language language, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,

                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK>(Guid id, Language language, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,

                ConstructorParameters = (new object[] { param1 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL>(Guid id, Language language, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,

                ConstructorParameters = (new object[] { param1, param2 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM>(Guid id, Language language, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,

                ConstructorParameters = (new object[] { param1, param2, param3 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM, TN>(Guid id, Language language, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,

                ConstructorParameters = (new object[] { param1, param2, param3, param4 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T>(Guid id, Language language, Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK>(Guid id, Language language, Sitecore.Data.Version version, TK param1, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL>(Guid id, Language language, Sitecore.Data.Version version, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1, param2 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM>(Guid id, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1, param2, param3 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("Use ISitecorethis.GetItem with builder")]
        public T GetItem<T, TK, TL, TM, TN>(Guid id, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByIdOptions
            {
                Id = id,
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType,
                Language = language,
                Version = version,
                ConstructorParameters = (new object[] { param1, param2, param3, param4 }).Select(x => new ConstructorParameter(x.GetType(), x)).ToArray(),
            };
            return this.GetItem<T>(options);
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
        [Obsolete("Use ISitecorethis.MoveItem")]
        public void Move<T, TK>(T item, TK newParent)
        {
            var options = new MoveByModelOptions()
            {
                Model = item,
                NewParent = newParent
            };

            this.MoveItem(options);
        }

        #endregion

        #region Query

        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        [Obsolete("Use ISitecorethis.GetItemsByQuery with builder")]
        public IEnumerable<T> Query<T>(string query, bool isLazy = false,
            bool inferType = false) where T : class
        {
            var options = new GetItemsByQueryOptions(Sc.Query.New(query))
            {
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType
            };
            return this.GetItems<T>(options);
        }


        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="language">The language of the items to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        [Obsolete("Use ISitecorethis.GetItemsByQuery with builder")]
        public IEnumerable<T> Query<T>(string query, Language language, bool isLazy = false, bool inferType = false)
            where T : class
        {
            var options = new GetItemsByQueryOptions(Sc.Query.New(query))
            {
                Lazy = GetLazyLoading(isLazy),
                Language = language,
                InferType = inferType
            };
            return this.GetItems<T>(options);
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
        [Obsolete("Use ISitecorethis.GetItemByQuery with builder")]
        public T QuerySingle<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            var options = new GetItemByQueryOptions()
            {
                Query = Sc.Query.New(query),
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType
            };
            return this.GetItem<T>(options);
        }


        #endregion

        #region Save

        /// <summary>
        /// Saves the specified target.
        /// </summary>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        [Obsolete("Use ISitecorethis.SaveItem with builder")]
        public void Save<T>(T target, bool updateStatistics = true,
            bool silent = false) where T : class
        {
            var options = new SaveOptions()
            {
                Model = target,
                UpdateStatistics = updateStatistics,
                Silent = silent
            };
            this.SaveItem(options);
        }

        #endregion

        #region WriteToItem

        /// <summary>
        /// Writes to item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <param name="target">The object to read data from.</param>
        /// <param name="item">The item to write data to.</param>
        [Obsolete("Use ISitecorethis.WriteToItem with builder")]
        public void WriteToItem<T>(T target, Item item, bool updateStatistics = true, bool silent = false) where T : class
        {
            this.WriteToItem(new WriteToItemOptions
            {
                Item = item,
                Model = target,
                UpdateStatistics = updateStatistics,
                Silent = silent
            });
        }

        #endregion


        /// <summary>
        /// Populate a model with data from Sitecore. The model must already have an ID property or ItemUri property with 
        /// a value already set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        [Obsolete("Use ISitecorthis.Populate")]
        public void Map<T>(T target) where T : class
        {
            this.Populate(target);
        }


        #endregion

    }
}




