/*
   Copyright 2011 Michael Edwards
 
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
using System.Linq;
using System.Text;
using System.Threading;
using Sitecore;
using Sitecore.Data.DataProviders;
using System.Xml;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Configuration;
using Sitecore.Caching;
using Sitecore.SecurityModel;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.CodeFirst
{
    public class GlassDataProvider : DataProvider
    {
        #region  IDs
        /// <summary>
        /// Taken from sitecore database
        /// </summary>
        private static readonly ID TemplateFolderId = new ID("{3C1715FE-6A13-4FCF-845F-DE308BA9741D}");


        #region Templates
        /// <summary>
        /// /sitecore/templates/System/Templates/Template section
        /// </summary>
        private static readonly ID SectionTemplateId = new ID("{E269FBB5-3750-427A-9149-7AA950B49301}");
        private static readonly ID FieldTemplateId = new ID("{455A3E98-A627-4B40-8035-E683A0331AC7}");
        private static readonly ID TemplateTemplateId = new ID("{AB86861A-6030-46C5-B394-E8F99E8B87DB}");
        private static readonly ID FolderTemplateId = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");

        #endregion

    

        #endregion

        

        public static readonly ID GlassFolderId = new ID("{19BC20D3-CCAB-4048-9CA9-4AA631AB109F}");

        public string DatabaseName { get; private set; }

        public Database Database { get { return Factory.GetDatabase(DatabaseName); } }


        private List<SectionInfo> SectionTable { get; set; }

        private List<FieldInfo> FieldTable { get; set; }

        private Context _glsContext;
        private string _contextName;

        public Dictionary<Type, SitecoreTypeConfiguration> _typeConfigurations;

        public GlassDataProvider()
        {
            SectionTable = new List<SectionInfo>();
            FieldTable = new List<FieldInfo>();

        }

        public GlassDataProvider(string databaseName, string context)
            : this()
        {

            DatabaseName = databaseName;

            _contextName = context;
           

        }

        public override global::Sitecore.Data.ItemDefinition GetItemDefinition(global::Sitecore.Data.ID itemId, CallContext context)
        {
            Setup(context);

            var section = SectionTable.FirstOrDefault(x => x.SectionId == itemId);
            if (section != null)
            {
                return  new ItemDefinition(itemId, section.Name, SectionTemplateId, ID.Null);
            }
            var field = FieldTable.FirstOrDefault(x => x.FieldId == itemId);
            if (field != null)
            {
                return new ItemDefinition(itemId, field.Name, FieldTemplateId, ID.Null);
            }


            return base.GetItemDefinition(itemId, context);
        }

        public override LanguageCollection GetLanguages(CallContext context)
        {
            return new LanguageCollection();
        }

        #region GetItemFields

        public override global::Sitecore.Data.FieldList GetItemFields(global::Sitecore.Data.ItemDefinition itemDefinition, global::Sitecore.Data.VersionUri versionUri, CallContext context)
        {
            Setup(context);

            FieldList fields = new FieldList();

            var sectionInfo = SectionTable.FirstOrDefault(x => x.SectionId == itemDefinition.ID);
            if (sectionInfo != null)
            {
                GetStandardFields(fields, sectionInfo.SectionSortOrder >= 0 ? sectionInfo.SectionSortOrder : (SectionTable.IndexOf(sectionInfo) + 100));

                return fields;
            }

            var fieldInfo = FieldTable.FirstOrDefault(x => x.FieldId == itemDefinition.ID);
            if (fieldInfo != null)
            {
                GetStandardFields(fields, fieldInfo.FieldSortOrder >= 0 ? fieldInfo.FieldSortOrder : (FieldTable.IndexOf(fieldInfo) + 100));
                GetFieldFields(fieldInfo, fields);
                return fields;
            }

            return base.GetItemFields(itemDefinition, versionUri, context);
        }

        private void GetStandardFields(FieldList fields, int index)
        {
            fields.Add(FieldIDs.ReadOnly, "1");
            fields.Add(FieldIDs.Sortorder, index.ToString());
        }

        private void GetFieldFields(FieldInfo info, FieldList fields)
        {

            if (!string.IsNullOrEmpty(info.Title))
                fields.Add(TemplateFieldIDs.Title, info.Title);

            fields.Add(TemplateFieldIDs.Type, FieldInfo.GetFieldType(info.Type));

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

        public override global::Sitecore.Collections.IDList GetChildIDs(global::Sitecore.Data.ItemDefinition itemDefinition, CallContext context)
        {
            Setup(context);

            if (_typeConfigurations == null)
                return base.GetChildIDs(itemDefinition, context);

            if (_typeConfigurations.Any(x => x.Value.TemplateId == itemDefinition.ID))
            {
                var cls = _typeConfigurations.First(x => x.Value.TemplateId == itemDefinition.ID).Value;
                return GetChildIDsTemplate(cls, itemDefinition, context);
            }

            var section = SectionTable.FirstOrDefault(x => x.SectionId == itemDefinition.ID);

            if (section != null)
            {
                return GetChildIDsSection(section, context);
            }

            return base.GetChildIDs(itemDefinition, context);
        }

        private IDList GetChildIDsTemplate(SitecoreTypeConfiguration template, ItemDefinition itemDefinition, CallContext context)
        {
            IDList fields = new IDList();

            List<string> processed = new List<string>();
            var sections = template.Properties
                .Where(x => x.PropertyInfo.DeclaringType == template.Type)
                .OfType<SitecoreFieldConfiguration>()
                .Select(x => new { x.SectionName, x.SectionSortOrder });

            var providers = Database.GetDataProviders();
            var otherProvider = providers.FirstOrDefault(x => !(x is GlassDataProvider));
            //If sitecore contains a section with the same name in the database, use that one instead of creating a new one
            var existing = otherProvider.GetChildIDs(itemDefinition, context).OfType<ID>().Select(id => otherProvider.GetItemDefinition(id, context)).ToList();

            foreach (var section in sections)
            {
                if (processed.Contains(section.SectionName) || section.SectionName.IsNullOrEmpty())
                    continue;

                var record = SectionTable.FirstOrDefault(x => x.TemplateId == itemDefinition.ID && x.Name == section.SectionName);

                if (record == null)
                {

                    var exists = existing.FirstOrDefault(def => def.Name.Equals(section));
                    if (exists != null)
                    {

                        record = new SectionInfo(section.SectionName, exists.ID, itemDefinition.ID, section.SectionSortOrder) { Existing = true };
                    }
                    else
                    {

                        record = new SectionInfo(section.SectionName, new ID(Guid.NewGuid()), itemDefinition.ID, section.SectionSortOrder);
                    }
                    SectionTable.Add(record);
                }
                processed.Add(section.SectionName);
                if (!record.Existing)
                    fields.Add(record.SectionId);
            }
            return fields;
        }

        private IDList GetChildIDsSection(SectionInfo section, CallContext context)
        {
            var cls = _typeConfigurations.First(x => x.Value.TemplateId == section.TemplateId).Value;

            var fields = cls.Properties.OfType<SitecoreFieldConfiguration>();

            IDList fieldIds = new IDList();

            var providers = Database.GetDataProviders();
            var otherProvider = providers.FirstOrDefault(x => !(x is GlassDataProvider));

            foreach (var field in fields)
            {
                if (field.PropertyInfo.DeclaringType != cls.Type)
                    continue;



                if (field.CodeFirst && field.SectionName == section.Name && !ID.IsNullOrEmpty(field.FieldId))
                {
                    var record = FieldTable.FirstOrDefault(x => x.FieldId == field.FieldId);
                    //test if the fields exists in the database: if so, we're using codefirst now, so remove it.
                    var existing = otherProvider.GetItemDefinition(field.FieldId, context);
                    if (existing != null)
                    {
                        using (new SecurityDisabler())
                            otherProvider.DeleteItem(existing, context);
                    }
                    if (record == null)
                    {
                        string fieldName = field.FieldName.IsNullOrEmpty() ? field.PropertyInfo.Name : field.FieldName;


                        record = new FieldInfo(field.FieldId, section.SectionId, fieldName, field.FieldType,
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

        public override global::Sitecore.Data.ID GetParentID(global::Sitecore.Data.ItemDefinition itemDefinition, CallContext context)
        {
            Setup(context);
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

        public override global::Sitecore.Data.ID GetRootID(CallContext context)
        {
            return base.GetRootID(context);
        }


        public override bool CreateItem(ID itemID, string itemName, ID templateID, ItemDefinition parent, CallContext context)
        {
            return false;
        }

        public override bool SaveItem(ItemDefinition itemDefinition, global::Sitecore.Data.Items.ItemChanges changes, CallContext context)
        {
            return false;
        }

        public override bool DeleteItem(ItemDefinition itemDefinition, CallContext context)
        {
            return false;
        }

        public readonly object _setupLock = new object();
        public bool _setupComplete = false;
        public bool _setupProcessing = false;

        public void Setup(CallContext context)
        {
            if (_setupComplete || _setupProcessing) return;

            lock (_setupLock)
            {
                if (_setupComplete || _setupProcessing || !Context.Contexts.ContainsKey(_contextName)) return;

                _setupProcessing = true;

                global::Sitecore.Diagnostics.Log.Info("Started CodeFirst setup", this);

               

                var providers = Factory.GetDatabase("master").GetDataProviders();
                var provider = providers.FirstOrDefault(x => !(x is GlassDataProvider));

                var templateFolder = provider.GetItemDefinition(TemplateFolderId, context);

                var glassFolder = provider.GetItemDefinition(GlassFolderId, context);

                if (glassFolder == ItemDefinition.Empty || glassFolder == null)
                {
                    provider.CreateItem(GlassFolderId, "GlassTemplates", FolderTemplateId, templateFolder, context);
                    glassFolder = provider.GetItemDefinition(GlassFolderId, context);
                }

                _glsContext = Context.Contexts[_contextName];
                _typeConfigurations = _glsContext.TypeConfigurations
                                            .Where(x => x.Value is SitecoreTypeConfiguration)
                                            .ToDictionary(x => x.Key, x => x.Value as SitecoreTypeConfiguration);

                foreach (var cls in _typeConfigurations.Where(x => x.Value.CodeFirst))
                {
                    var clsTemplate = provider.GetItemDefinition(cls.Value.TemplateId, context);

                    if (clsTemplate == ItemDefinition.Empty || clsTemplate == null)
                    {

                        //setup folders
                        IEnumerable<string> namespaces = cls.Key.Namespace.Split('.');
                        namespaces = namespaces.SkipWhile(x => x != "Templates").Skip(1);

                        ItemDefinition containing = glassFolder;
                        var children = provider.GetChildIDs(containing, context);
                        foreach (var ns in namespaces)
                        {
                            ItemDefinition found = null;
                            foreach (ID child in children)
                            {
                                if (!ID.IsNullOrEmpty(child))
                                {
                                    var childDef = provider.GetItemDefinition(child, context);
                                    if (childDef.Name == ns)
                                        found = childDef;
                                }
                            }

                            if (found == null)
                            {
                                ID newId = ID.NewID;
                                provider.CreateItem(newId, ns, FolderTemplateId, containing, context);
                                found = provider.GetItemDefinition(newId, context);
                            }
                            containing = found;

                        }

                        //create the template in Sitecore
                        string templateName = cls.Value.TemplateName;

                        if (string.IsNullOrEmpty(templateName))
                            templateName = cls.Key.Name;

                        provider.CreateItem(cls.Value.TemplateId, templateName, TemplateTemplateId, containing, context);
                        clsTemplate = provider.GetItemDefinition(cls.Value.TemplateId, context);
                        //Assign the base template
                        var templateItem = Factory.GetDatabase("master").GetItem(clsTemplate.ID);

                        using (new SecurityDisabler())
                        {
                            templateItem.Editing.BeginEdit();
                            templateItem["__Base template"] = "{1930BBEB-7805-471A-A3BE-4858AC7CF696}";
                            templateItem.Editing.EndEdit();
                        }
                    }

                    BaseTemplateChecks(clsTemplate, provider, context, cls.Value);

                    //initialize sections and children
                    foreach (ID sectionId in this.GetChildIDsTemplate(cls.Value, clsTemplate, context))
                    {
                        this.GetChildIDsSection(SectionTable.First(s => s.SectionId == sectionId), context);
                    }
                }

                if (global::Sitecore.Configuration.Settings.GetBoolSetting("AutomaticallyRemoveDeletedTemplates", true))
                    RemoveDeletedClasses(glassFolder, provider, context);

                global::Sitecore.Diagnostics.Log.Info("Finished CodeFirst setup", this);

              

                _setupComplete = true;
            }
        }

        /// <summary>
        /// Check a folder and all sub folders in Sitecore for templates
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="provider"></param>
        /// <param name="context"></param>
        /// <returns>True of the folder is deleted itself.</returns>
        private bool RemoveDeletedClasses(ItemDefinition folder, DataProvider provider, CallContext context)
        {
            var childIds = provider.GetChildIDs(folder, context);

            //check child items
            foreach (ID childId in childIds.ToArray())
            {
                var childDefinition = provider.GetItemDefinition(childId, context);

                //if child is template check the template still exists in the code base
                if (childDefinition.TemplateID == TemplateTemplateId)
                {
                    if (_typeConfigurations.Any(x => x.Value.TemplateId == childDefinition.ID && x.Value.CodeFirst))
                        continue;

                    provider.DeleteItem(childDefinition, context);
                    childIds.Remove(childDefinition.ID);
                }
                // if the child is a folder check the children of the folder
                else if (childDefinition.TemplateID == FolderTemplateId)
                {
                    //if the folder itself is deleted then remove from the parent
                    if (RemoveDeletedClasses(childDefinition, provider, context))
                    {
                        childIds.Remove(childDefinition.ID);
                    }
                }
            }

            //if there are no children left delete the folder 
            if (childIds.Count == 0)
            {
                provider.DeleteItem(folder, context);
                return true;
            }
            else
            {
                return false;
            }



        }

        private void BaseTemplateChecks(ItemDefinition template, DataProvider provider, CallContext context, SitecoreTypeConfiguration config)
        {
            //check base templates



            var templateItem = Factory.GetDatabase("master").GetItem(template.ID);


            var baseTemplatesField = templateItem[FieldIDs.BaseTemplate];
            StringBuilder sb = new StringBuilder(baseTemplatesField);

            global::Sitecore.Diagnostics.Log.Info("Type {0}".Formatted(config.Type.FullName), this);


            Action<Type> idCheck = (type) =>
            {
                global::Sitecore.Diagnostics.Log.Info("ID Check {0}".Formatted(type.FullName), this);

                if (!_typeConfigurations.ContainsKey(type)) return;

                var baseConfig = _typeConfigurations[type];
                if (baseConfig != null && baseConfig.CodeFirst)
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



            config.Type.GetInterfaces().ForEach(x => idCheck(x));

            if (baseTemplatesField != sb.ToString())
            {
                templateItem.Editing.BeginEdit();
                templateItem[FieldIDs.BaseTemplate] = sb.ToString();
                templateItem.Editing.EndEdit();
            }


        }
    }
}
