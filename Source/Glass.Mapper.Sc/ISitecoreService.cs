using System;
using System.Collections;
using System.Collections.Generic;
using Glass.Mapper.Sc.Builders;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Interface ISitecoreService
    /// </summary>
    public interface ISitecoreService : IAbstractService, ISitecoreServiceLegacy
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        Database Database { get; }

        /// <summary>
        /// 
        /// </summary>
        Config Config { get; set; }

        /// <summary>
        /// Adds a version of the item
        /// </summary>
        /// <typeparam name="T">The type being added. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="target">The class to add a version to</param>
        /// <returns>``0.</returns>
        T AddVersion<T>(T target) where T : class;

        /// <summary>
        /// Writes to item.
        /// </summary>
        void WriteToItem(WriteToItemOptions options);

        /// <summary>
        /// Populate a model with data from Sitecore. The model must already have an ID property or ItemUri property with 
        /// a value already set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        void Populate<T>(T target) where T : class;

        /// <summary>
        /// Populate a model with data from Sitecore. The model must already have an ID property or ItemUri property with 
        /// a value already set.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="target">The object to populate</param>
        /// <param name="options">Options for how the model will be retrieved</param>
        void Populate<T>(T target, GetOptions options) where T : class;

        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <param name="abstractTypeSavingContext">The abstract type saving context.</param>
        void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext);

        /// <summary>
        /// Returns the item that the specific object relates to
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        Item ResolveItem(object target);

        /// <summary>
        /// Map a collection items to the requested type.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the models will be retrieved</param>
        /// <returns></returns>
        IEnumerable<T> GetItems<T>(GetItemsOptions options) where T : class;

        /// <summary>
        /// Map a collection items to the requested type.
        /// </summary>
        /// <param name="options">Options for how the models will be retrieved</param>
        /// <returns></returns>
        IEnumerable<object> GetItems(GetItemsOptions options);
      
        /// <summary>
        /// Map an item to the requested type.
        /// </summary>
        /// <param name="options">Options for how the model will be retrieved</param>
        object GetItem(GetItemOptions options);

        /// <summary>
        /// Move an item in the Sitecore content tree.
        /// </summary>
        /// <param name="options">Options for how the model will be moved.</param>
        void MoveItem(MoveByModelOptions options);

        /// <summary>
        /// Delete an item in the Sitecore content tree.
        /// </summary>
        /// <param name="options">Options for how the model will be deleted.</param>
        void DeleteItem(DeleteByModelOptions options);

        /// <summary>
        /// Map an item to the requested type.
        /// </summary>
        /// <param name="options">Options for how the model will be retrieved</param>
        T GetItem<T>(GetItemOptions options) where T : class;

        /// <summary>
        /// Save an item to the Sitecore database.
        /// </summary>
        /// <param name="options">Options for how the model will be saved</param>
        void SaveItem(SaveOptions options);

        /// <summary>
        /// Create an item to the Sitecore database.
        /// </summary>
        /// <param name="options">Options for how the model will be Created</param>
        object CreateItem(CreateOptions options);

        /// <summary>
        /// Create an item to the Sitecore database.
        /// </summary>
        /// <param name="options">Options for how the model will be Created</param>
        T CreateItem<T>(CreateOptions options)
            where T : class;

    }



    
   
  

   

  






}




