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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.DataProviders;
using Sitecore.Data.DataProviders.Sql;
using Sitecore.Data.Items;
using Sitecore.Exceptions;
using Sitecore.SecurityModel;
using Sitecore.Shell.Feeds.Sections;

namespace Glass.Mapper.Sc.CodeFirst
{
    /// <summary>
    /// Class GlassDataProvider
    /// </summary>
    public class GlassDataProvider : DataProvider
    {
        public static bool DisableItemHandlerWhenDeletingFields = false;
        private static readonly object _setupLock = new object();
        private bool _setupComplete;
        private bool _setupProcessing;

        #region  IDs

        /// <summary>
        /// Taken from sitecore database
        /// </summary>
        private static readonly ID TemplateFolderId = new ID("{3C1715FE-6A13-4FCF-845F-DE308BA9741D}");

        /// <summary>
        /// The glass folder id
        /// </summary>
        public static ID GlassFolderId = new ID("{19BC20D3-CCAB-4048-9CA9-4AA631AB109F}");

        #region Templates

        /// <summary>
        /// /sitecore/templates/System/Templates/Template section
        /// </summary>
        private static readonly ID SectionTemplateId = new ID("{E269FBB5-3750-427A-9149-7AA950B49301}");
        /// <summary>
        /// The field template id
        /// </summary>
        private static readonly ID FieldTemplateId = new ID("{455A3E98-A627-4B40-8035-E683A0331AC7}");
        /// <summary>
        /// The template template id
        /// </summary>
        private static readonly ID TemplateTemplateId = new ID("{AB86861A-6030-46C5-B394-E8F99E8B87DB}");
        /// <summary>
        /// The folder template id
        /// </summary>
        private static readonly ID FolderTemplateId = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");

        /// <summary>
        /// The ID of the base templates field
        /// </summary>
        public static ID BaseTemplateField = new ID("{12C33F3F-86C5-43A5-AEB4-5598CEC45116}");

        #endregion

        #endregion

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// The name of the GlassContext to load
        /// </summary>
        public string ContextName { get; set; }

        /// <summary>
        /// Gets or sets the section table.
        /// </summary>
        /// <value>The section table.</value>
        private List<SectionInfo> SectionTable { get; set; }

        /// <summary>
        /// Gets or sets the field table.
        /// </summary>
        /// <value>The field table.</value>
        private List<FieldInfo> FieldTable { get; set; }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>
        /// The current context.
        /// </value>
        public Context CurrentContext
        {
            get { return Context.Contexts[ContextName]; }
        }

        /// <summary>
        /// Gets the type configurations.
        /// </summary>
        /// <value>
        /// The type configurations.
        /// </value>
        public Dictionary<Type, SitecoreTypeConfiguration> TypeConfigurations
        {
            get
            {
                return CurrentContext.TypeConfigurations
                    .Where(x => x.Value is SitecoreTypeConfiguration)
                    .ToDictionary(x => x.Key, x => x.Value as SitecoreTypeConfiguration);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassDataProvider"/> class.
        /// </summary>
        public GlassDataProvider()
        {
            SectionTable = new List<SectionInfo>();
            FieldTable = new List<FieldInfo>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassDataProvider"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="context">The context.</param>
        public GlassDataProvider(string databaseName, string context)
            : this()
        {
            DatabaseName = databaseName;
            ContextName = context;
        }

        /// <summary>
        /// Gets the definition of an item.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="context">The context.</param>
        /// <returns>Sitecore.Data.ItemDefinition.</returns>
        public override ItemDefinition GetItemDefinition(ID itemId, CallContext context)
        {
            if (!_setupComplete)
                return base.GetItemDefinition(itemId, context);

            var section = SectionTable.FirstOrDefault(x => x.SectionId == itemId);
            if (section != null)
            {
                return new ItemDefinition(itemId, section.Name, SectionTemplateId, ID.Null);
            }

            var field = FieldTable.FirstOrDefault(x => x.FieldId == itemId);
            if (field != null)
            {
                return new ItemDefinition(itemId, field.Name, FieldTemplateId, ID.Null);
            }

            return base.GetItemDefinition(itemId, context);
        }

        /// <summary>
        /// Gets a list of all the language that have been defined
        /// in the database.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>LanguageCollection.</returns>
        public override LanguageCollection GetLanguages(CallContext context)
        {
            return new LanguageCollection();
        }

        #region GetItemFields

        /// <summary>
        /// Gets the fields of a specific item version.
        /// </summary>
        /// <param name="itemDefinition">The item.</param>
        /// <param name="versionUri">The version URI.</param>
        /// <param name="context">The context.</param>
        /// <returns>Sitecore.Data.FieldList.</returns>
        public override FieldList GetItemFields(ItemDefinition itemDefinition, VersionUri versionUri, CallContext context)
        {
            if (!_setupComplete)
                return base.GetItemFields(itemDefinition, versionUri, context);

            var fields = new FieldList();

            var sectionInfo = SectionTable.FirstOrDefault(x => x.SectionId == itemDefinition.ID);
            if (sectionInfo != null)
            {
                GetStandardFields(fields,
                    sectionInfo.SectionSortOrder >= 0
                        ? sectionInfo.SectionSortOrder
                        : (SectionTable.IndexOf(sectionInfo) + 100));

                return fields;
            }

            var fieldInfo = FieldTable.FirstOrDefault(x => x.FieldId == itemDefinition.ID);
            if (fieldInfo != null)
            {
                GetStandardFields(fields,
                    fieldInfo.FieldSortOrder >= 0 ? fieldInfo.FieldSortOrder : (FieldTable.IndexOf(fieldInfo) + 100));
                GetFieldFields(fieldInfo, fields);
                return fields;
            }

            return base.GetItemFields(itemDefinition, versionUri, context);
        }

        /// <summary>
        /// Gets the standard fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="index">The index.</param>
        private void GetStandardFields(FieldList fields, int index)
        {
            fields.Add(FieldIDs.ReadOnly, "1");
            fields.Add(FieldIDs.Sortorder, index.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets the field fields.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="fields">The fields.</param>
        private void GetFieldFields(FieldInfo info, FieldList fields)
        {
            if (!string.IsNullOrEmpty(info.Title))
                fields.Add(TemplateFieldIDs.Title, info.Title);

            fields.Add(TemplateFieldIDs.Type, info.GetFieldType());

            if (!string.IsNullOrEmpty(info.Source))
                fields.Add(TemplateFieldIDs.Source, info.Source);

            fields.Add(TemplateFieldIDs.Shared, info.IsShared ? "1" : "0");
            fields.Add(TemplateFieldIDs.Unversioned, info.IsUnversioned ? "1" : "0");

            foreach (var fieldFieldValue in info.FieldFieldValues)
            {
                fields.Add(ID.Parse(fieldFieldValue.Key), fieldFieldValue.Value);
            }

            fields.Add(TemplateFieldIDs.Validation, info.ValidationRegularExpression ?? "");
            fields.Add(TemplateFieldIDs.ValidationText, info.ValidationErrorText ?? "");

            if (info.IsRequired)
            {
                fields.Add(Global.IDs.TemplateFieldIds.QuickActionBarFieldId, Global.IDs.TemplateFieldIds.IsRequiredId);
                fields.Add(Global.IDs.TemplateFieldIds.ValidateButtonFieldId, Global.IDs.TemplateFieldIds.IsRequiredId);
                fields.Add(Global.IDs.TemplateFieldIds.ValidatorBarFieldId, Global.IDs.TemplateFieldIds.IsRequiredId);
                fields.Add(Global.IDs.TemplateFieldIds.WorkflowFieldId, Global.IDs.TemplateFieldIds.IsRequiredId);
            }
        }

        #endregion

        #region GetChildIDs

        /// <summary>
        /// Gets the child ids of an item.
        /// </summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <param name="context">The context.</param>
        /// <returns>Sitecore.Collections.IDList.</returns>
        public override IDList GetChildIDs(ItemDefinition itemDefinition, CallContext context)
        {
            if (!_setupComplete)
                return base.GetChildIDs(itemDefinition, context);

            if (TypeConfigurations.Any(x => x.Value.TemplateId == itemDefinition.ID))
            {
                var cls = TypeConfigurations.First(x => x.Value.TemplateId == itemDefinition.ID).Value;
                return GetChildIDsTemplate(cls, itemDefinition, context, GetSqlProvider(context.DataManager.Database));
            }

            var section = SectionTable.FirstOrDefault(x => x.SectionId == itemDefinition.ID);

            if (section != null)
            {
                return GetChildIDsSection(section, context, GetSqlProvider(context.DataManager.Database));
            }

            return base.GetChildIDs(itemDefinition, context);
        }

        /// <summary>
        /// Gets the child I ds template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="itemDefinition">The item definition.</param>
        /// <param name="context">The context.</param>
        /// <param name="sqlProvider">The SQL provider.</param>
        /// <returns>
        /// IDList.
        /// </returns>
        private IDList GetChildIDsTemplate(SitecoreTypeConfiguration template, ItemDefinition itemDefinition, CallContext context, DataProvider sqlProvider)
        {
            var fields = new IDList();
            var processed = new List<string>();
            var sections = template.Properties
                .Where(x => x.PropertyInfo.DeclaringType == template.Type)
                .OfType<SitecoreFieldConfiguration>()
                .Select(x => new { x.SectionName, x.SectionSortOrder });

            //If sitecore contains a section with the same name in the database, use that one instead of creating a new one
            var existing = sqlProvider.GetChildIDs(itemDefinition, context).OfType<ID>().Select(id => sqlProvider.GetItemDefinition(id, context))
                .Where(item => item.TemplateID == SectionTemplateId).ToList();

            foreach (var section in sections)
            {
                if (processed.Contains(section.SectionName) || section.SectionName.IsNullOrEmpty())
                    continue;

                var record = SectionTable.FirstOrDefault(x => x.TemplateId == itemDefinition.ID && x.Name == section.SectionName);

                if (record == null)
                {
                    var exists = existing.FirstOrDefault(def => def.Name.Equals(section.SectionName, StringComparison.InvariantCultureIgnoreCase));
                    var newId = GetUniqueGuid(itemDefinition.ID + section.SectionName);
                    const int newSortOrder = 100;
                    
                    record = exists != null ?
                        new SectionInfo(section.SectionName, exists.ID, itemDefinition.ID, section.SectionSortOrder) { Existing = true } :
                        new SectionInfo(section.SectionName, new ID(newId), itemDefinition.ID, newSortOrder);

                    SectionTable.Add(record);
                }

                processed.Add(section.SectionName);

                if (!record.Existing)
                    fields.Add(record.SectionId);
            }

            //we need to add sections already in the db, 'cause we have to 
            foreach (var sqlOne in existing.Where(ex => SectionTable.All(s => s.SectionId != ex.ID)))
            {
                SectionTable.Add(new SectionInfo(sqlOne.Name, sqlOne.ID, itemDefinition.ID, 0) { Existing = true } );
            }

            return fields;
        }

        /// <summary>
        /// Gets the child I ds section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="context">The context.</param>
        /// <param name="sqlProvider">The SQL provider.</param>
        /// <returns>
        /// IDList.
        /// </returns>
        private IDList GetChildIDsSection(SectionInfo section, CallContext context, DataProvider sqlProvider)
        {
            var config = TypeConfigurations.First(x => x.Value.TemplateId == section.TemplateId);
            var cls = config.Value;

            var fields = cls.Properties.OfType<SitecoreFieldConfiguration>();

            IDList fieldIds = new IDList();

            var interfaces = cls.Type.GetInterfaces();

            foreach (var field in fields)
            {
                //fix: added check on interfaces, if field resides on interface then skip here
                var propertyFromInterface = interfaces.FirstOrDefault(inter => inter.GetProperty(field.PropertyInfo.Name) != null 
                                                                            && inter.GetProperty(field.PropertyInfo.Name).GetCustomAttributes(typeof(SitecoreFieldAttribute), false).Any());
                if (field.PropertyInfo.DeclaringType != cls.Type || propertyFromInterface != null)
                    continue;

                if (field.CodeFirst && field.SectionName == section.Name && !ID.IsNullOrEmpty(field.FieldId))
                {
                    var record = FieldTable.FirstOrDefault(x => x.FieldId == field.FieldId);
                    //test if the fields exists in the database: if so, we're using codefirst now, so remove it.
                    var existing = sqlProvider.GetItemDefinition(field.FieldId, context);
                    if (existing != null)
                    {
                        using (new SecurityDisabler())
                        {
                            if (DisableItemHandlerWhenDeletingFields)
                            {
                                using (new DisableItemHandler())
                                    sqlProvider.DeleteItem(existing, context);
                            }
                            else
                            {
                                sqlProvider.DeleteItem(existing, context);
                            }
                        }
                    }

                    if (record == null)
                    {
                        string fieldName = field.FieldName.IsNullOrEmpty() ? field.PropertyInfo.Name : field.FieldName;

                        record = new FieldInfo(field.FieldId, section.SectionId, fieldName, field.FieldType, field.CustomFieldType,
                                               field.FieldSource, field.FieldTitle, field.IsShared, field.IsUnversioned,
                                               field.FieldSortOrder, field.ValidationRegularExpression,
                                               field.ValidationErrorText, field.IsRequired);

                        if (field.FieldValueConfigs != null && field.FieldValueConfigs.Any())
                        {
                            foreach (var ffv in field.FieldValueConfigs)
                            {
                                record.FieldFieldValues.Add(ffv.FieldId, ffv.FieldValue);
                            }
                        }
                    }

                    fieldIds.Add(record.FieldId);
                    FieldTable.Add(record);
                }
            }

            return fieldIds;
        }

        #endregion

        /// <summary>
        /// Gets the parent ID of an item.
        /// </summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <param name="context">The context.</param>
        /// <returns>Sitecore.Data.ID.</returns>
        public override ID GetParentID(ItemDefinition itemDefinition, CallContext context)
        {
            if (!_setupComplete)
                return base.GetParentID(itemDefinition, context);

            var section = SectionTable.FirstOrDefault(x => x.SectionId == itemDefinition.ID);

            if (section != null)
            {
                return section.TemplateId;
            }

            var field = FieldTable.FirstOrDefault(x => x.FieldId == itemDefinition.ID);
            if (field != null)
            {
                return field.SectionId;
            }

            return base.GetParentID(itemDefinition, context);
        }

        /// <summary>
        /// Creates a item.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="templateId">The template ID.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public override bool CreateItem(ID itemId, string itemName, ID templateId, ItemDefinition parent, CallContext context)
        {
            return false;
        }

        /// <summary>
        /// Saves an item.
        /// </summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <param name="changes">The changes.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public override bool SaveItem(ItemDefinition itemDefinition, ItemChanges changes, CallContext context)
        {
            return false;
        }

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public override bool DeleteItem(ItemDefinition itemDefinition, CallContext context)
        {
            return false;
        }

        /// <summary>
        /// Setups the specified context.
        /// </summary>
        /// <param name="db">The db.</param>
        public void Initialise(Database db)
        {
            if (_setupComplete || _setupProcessing)
                return;

            lock (_setupLock)
            {
                try
                {
                    if (_setupComplete || _setupProcessing)
                        return;

                    Sitecore.Diagnostics.Log.Info("Started CodeFirst setup " + db.Name, this);
                    _setupProcessing = true;
                    var manager = new DataManager(db);
                    var context = new CallContext(manager, db.GetDataProviders().Count());

                    var sqlProvider = GetSqlProvider(db);
                    var glassFolder = GetGlassTemplateFolder(sqlProvider, context);

                    if (CurrentContext != null)
                    {
                        foreach (var cls in TypeConfigurations.Where(x => x.Value.CodeFirst))
                        {
                            var templateDefinition = sqlProvider.GetItemDefinition(cls.Value.TemplateId, context);

                            if (templateDefinition == ItemDefinition.Empty || templateDefinition == null)
                            {
                                var containingFolder = GetTemplateFolder(cls.Key.Namespace, glassFolder, sqlProvider, context);
                                templateDefinition = CreateTemplateItem(db, cls.Value, cls.Key, sqlProvider, containingFolder, context);
                            }

                            BaseTemplateChecks(templateDefinition, cls.Value, db);

                            //initialize sections and children
                            foreach (ID sectionId in GetChildIDsTemplate(cls.Value, templateDefinition, context, sqlProvider))
                            {
                                GetChildIDsSection(SectionTable.First(s => s.SectionId == sectionId), context, sqlProvider);
                            }
                        }

                        if (Settings.GetBoolSetting("AutomaticallyRemoveDeletedTemplates", true))
                            RemoveDeletedClasses(glassFolder, sqlProvider, context);

                        ClearCaches(db);
                    }

                    Sitecore.Diagnostics.Log.Info("Finished CodeFirst setup " + db.Name, this);
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("CodeFirst error " + ex.Message, ex, this);
                    throw;
                }
                finally
                {
                    _setupComplete = true;
                }
            }
        }

        /// <summary>
        /// Creates the template item.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="config">The config.</param>
        /// <param name="type">The type.</param>
        /// <param name="sqlDataProvider">The SQL data provider.</param>
        /// <param name="containingFolder">The containing folder.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="Sitecore.Exceptions.RequiredObjectIsNullException">TemplateItem is null for ID {0}.Formatted(templateDefinition.ID)</exception>
        private ItemDefinition CreateTemplateItem(
            Database db,
            SitecoreTypeConfiguration config,
            Type type,
            DataProvider sqlDataProvider,
            ItemDefinition containingFolder,
            CallContext context)
        {
            //create the template in Sitecore
            string templateName = string.IsNullOrEmpty(config.TemplateName) ? type.Name : config.TemplateName;
            sqlDataProvider.CreateItem(config.TemplateId, templateName, TemplateTemplateId, containingFolder, context);

            var templateDefinition = sqlDataProvider.GetItemDefinition(config.TemplateId, context);
            ClearCaches(db);

            var templateItem = db.GetItem(templateDefinition.ID);

            if (templateItem == null)
                throw new RequiredObjectIsNullException("TemplateItem is null for ID {0}".Formatted(templateDefinition.ID));

            using (new ItemEditing(templateItem, true))
            {
                templateItem["__Base template"] = "{1930BBEB-7805-471A-A3BE-4858AC7CF696}";
            }

            return templateDefinition;
        }

        /// <summary>
        /// Gets the template folder that the class template should be created in using the classes
        /// namespace. 
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="defaultFolder"></param>
        /// <param name="sqlDataProvider"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private ItemDefinition GetTemplateFolder(string nameSpace, ItemDefinition defaultFolder, DataProvider sqlDataProvider, CallContext context)
        {
            //setup folders
            IEnumerable<string> namespaces = nameSpace.Split('.');
            namespaces = namespaces.SkipWhile(x => x != "Templates").Skip(1);
            ItemDefinition containingFolder = defaultFolder;

            foreach (var ns in namespaces)
            {
                var children = sqlDataProvider.GetChildIDs(containingFolder, context);
                ItemDefinition found = null;

                foreach (ID child in children)
                {
                    if (!ID.IsNullOrEmpty(child))
                    {
                        var childDef = sqlDataProvider.GetItemDefinition(child, context);
                        if (childDef.Name == ns)
                            found = childDef;
                    }
                }

                if (found == null)
                {
                    ID newId = ID.NewID;
                    sqlDataProvider.CreateItem(newId, ns, FolderTemplateId, containingFolder, context);
                    found = sqlDataProvider.GetItemDefinition(newId, context);
                }

                containingFolder = found;
            }

            if (containingFolder == null)
            {
                Sitecore.Diagnostics.Log.Error("Failed to load containing folder {0}".Formatted(nameSpace), this);
                throw new RequiredObjectIsNullException("Failed to load containing folder {0}".Formatted(nameSpace));
            }

            return containingFolder;
        }

        /// <summary>
        /// Clears the all caches on the database
        /// </summary>
        /// <param name="db"></param>
        private void ClearCaches(Database db)
        {
            db.Caches.DataCache.Clear();
            db.Caches.ItemCache.Clear();
            db.Caches.ItemPathsCache.Clear();
            db.Caches.StandardValuesCache.Clear();
            db.Caches.PathCache.Clear();
        }

        /// <summary>
        /// Gets the base SQL provider that will store physical items
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private DataProvider GetSqlProvider(Database db)
        {
            var providers = db.GetDataProviders();
            var provider = providers.FirstOrDefault(x => x is SqlDataProvider);

            if (provider == null)
            {
                Sitecore.Diagnostics.Log.Error("Failed to find SqlDataProvider", this);
                throw new RequiredObjectIsNullException("Failed to find SqlDataProvider");
            }

            return provider;
        }

        /// <summary>
        /// Creates the item /sitecore/templates/glasstemplates
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private ItemDefinition GetGlassTemplateFolder(DataProvider provider, CallContext context)
        {
            var templateFolder = provider.GetItemDefinition(TemplateFolderId, context);
            var glassFolder = provider.GetItemDefinition(GlassFolderId, context);

            if (glassFolder == ItemDefinition.Empty || glassFolder == null)
            {
                provider.CreateItem(GlassFolderId, "GlassTemplates", FolderTemplateId, templateFolder, context);
                glassFolder = provider.GetItemDefinition(GlassFolderId, context);
            }

            if (glassFolder == null)
            {
                Sitecore.Diagnostics.Log.Error("Failed to find GlassTemplates folder", this);
                throw new RequiredObjectIsNullException("Failed to find GlassTemplates folder");
            }

            return glassFolder;
        }

        /// <summary>
        /// Check a folder and all sub folders in Sitecore for templates
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="sqlDataProvider">The SQL data provider.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// True of the folder is deleted itself.
        /// </returns>
        private bool RemoveDeletedClasses(ItemDefinition folder, DataProvider sqlDataProvider, CallContext context)
        {
            if (folder == null) throw new ArgumentNullException("folder");
            if (sqlDataProvider == null) throw new ArgumentNullException("sqlDataProvider");
            if (context == null) throw new ArgumentNullException("context");

            var childIds = sqlDataProvider.GetChildIDs(folder, context);

            //check child items
            foreach (ID childId in childIds.ToArray())
            {
                var childDefinition = sqlDataProvider.GetItemDefinition(childId, context);

                //if child is template check the template still exists in the code base
                if (childDefinition.TemplateID == TemplateTemplateId)
                {
                    if (TypeConfigurations.Any(x => x.Value.TemplateId == childDefinition.ID && x.Value.CodeFirst))
                        continue;

                    sqlDataProvider.DeleteItem(childDefinition, context);
                    childIds.Remove(childDefinition.ID);
                }
                // if the child is a folder check the children of the folder
                else if (childDefinition.TemplateID == FolderTemplateId)
                {
                    //if the folder itself is deleted then remove from the parent
                    if (RemoveDeletedClasses(childDefinition, sqlDataProvider, context))
                    {
                        childIds.Remove(childDefinition.ID);
                    }
                }
            }

            //if there are no children left delete the folder 
            if (childIds.Count == 0 && folder.ID != GlassFolderId)
            {
                sqlDataProvider.DeleteItem(folder, context);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bases the template checks.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="config">The config.</param>
        /// <param name="db">The database.</param>
        private void BaseTemplateChecks(
            ItemDefinition template,
            SitecoreTypeConfiguration config,
            Database db)
        {
            //check base templates
            var templateItem = db.GetItem(template.ID);
            var baseTemplatesField = templateItem[FieldIDs.BaseTemplate];
            var sb = new StringBuilder(baseTemplatesField);

            Sitecore.Diagnostics.Log.Info("Type {0}".Formatted(config.Type.FullName), this);

            Action<Type> idCheck = type =>
            {
                Sitecore.Diagnostics.Log.Info("ID Check {0}".Formatted(type.FullName), this);

                if (!TypeConfigurations.ContainsKey(type)) return;

                var baseConfig = TypeConfigurations[type];
                if (baseConfig != null && baseConfig.TemplateId.Guid != Guid.Empty)
                {
                    if (!baseTemplatesField.Contains(baseConfig.TemplateId.ToString()))
                    {
                        sb.Append("|{0}".Formatted(baseConfig.TemplateId));
                    }
                }
            };

            Type baseType = config.Type.BaseType;

            while (baseType != null)
            {
                idCheck(baseType);    
                baseType = baseType.BaseType;
            }

            config.Type.GetInterfaces().ForEach(idCheck);

            //dirty fix for circular template inheritance
            var baseTemplates = sb.ToString().Split('|').ToList();
            baseTemplates.Remove(config.TemplateId.ToString());
            sb.Clear();
            sb.Append(string.Join("|", baseTemplates));

            if (baseTemplatesField != sb.ToString())
            {
                templateItem.Editing.BeginEdit();
                templateItem[FieldIDs.BaseTemplate] = sb.ToString();
                templateItem.Editing.EndEdit();
            }
        }

        public static Guid GetUniqueGuid(string input)
        {
            //this code will generate a unique Guid for a string (unique with a 2^20.96 probability of a collision) 
            //http://stackoverflow.com/questions/2190890/how-can-i-generate-guid-for-a-string-values
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                var guid = new Guid(hash);
                return guid == Guid.Empty ? Guid.NewGuid() : guid;
            }

        }
    }
}

