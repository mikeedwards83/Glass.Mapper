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
using System.Linq;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data.Items;
using System.Dynamic;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.Dynamic
{
    /// <summary>
    /// Class DynamicItem
    /// </summary>
    public class DynamicItem : DynamicObject, IDynamicItem
    {
        Item _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public DynamicItem(Item item)
        {
            _item = item;
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.Object.</returns>
        protected virtual object GetField(string fieldName, Item item){
            return new DynamicField(fieldName, item);
        }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>DynamicItem.</returns>
        protected virtual DynamicItem CreateNew(Item item)
        {
            return new DynamicItem(item);
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)</returns>
        /// <exception cref="System.NotSupportedException">No field of Sitecore info matches the name {0} for item {1}.Formatted(name, _item.Paths.FullPath)</exception>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            string name = binder.Name;

            if (name == "Id")
            {
                result = _item.ID;
                return true;
            }


            if (_item.Fields[name] != null)
            {
                result = GetField(name, _item);
               return true;
            }

            SitecoreInfoType infoType;

            if (Enum.TryParse<SitecoreInfoType>(name, out infoType))
            {
                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = infoType;

                mapper.Setup(new DataMapperResolverArgs(null, config ));
                result = mapper.MapToProperty(new SitecoreDataMappingContext(null, _item, null));
                return true;
            }

           

            switch (name)
            {
                case "Parent":
                    result = Parent;
                    break;
                case "Children":
                    result = Children;
                    break;
            }



            if (result == null)
            {
                result = string.Empty;
            }

            return result != null;

            //throw new NotSupportedException("No field of Sitecore info matches the name {0} for item {1}".Formatted(name, _item.Paths.FullPath));

        }

        private T GetInfo<T>(SitecoreInfoType infoType)
        {
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = infoType;
          //  config.PropertyInfo = new FakePropertyInfo(typeof(T),typeof(DynamicItem));
            mapper.Setup(new DataMapperResolverArgs(null, config));
            return (T) mapper.MapToProperty(new SitecoreDataMappingContext(null, _item, null));
        }



        public IDynamicItem Parent
        {
            get { return CreateNew(_item.Parent); }
        }

        public IEnumerable<IDynamicItem> Children
        {
            get
            {
               return new DynamicCollection<DynamicItem>(_item.Children.Select(x => CreateNew(x)).ToArray());
            }
        }

        public string ContentPath
        {
            get { return GetInfo<string>(SitecoreInfoType.ContentPath); }
        }
        public string DisplayName
        {
            get { return GetInfo<string>(SitecoreInfoType.DisplayName); }
        }
        public string FullPath
        {
            get { return GetInfo<string>(SitecoreInfoType.FullPath); }
        }
        public string Key
        {
            get { return GetInfo<string>(SitecoreInfoType.Key); }
        }
        public string MediaUrl
        {
            get { return GetInfo<string>(SitecoreInfoType.MediaUrl); }
        }
        public string Path
        {
            get { return GetInfo<string>(SitecoreInfoType.Path); }
        }
        public Guid TemplateId
        {
            get { return GetInfo<Guid>(SitecoreInfoType.TemplateId); }
        }
        public string TemplateName
        {
            get { return GetInfo<string>(SitecoreInfoType.TemplateName); }
        }
        public string Url
        {
            get { return GetInfo<string>(SitecoreInfoType.Url); }
        }

        public int Version
        {
            get { return GetInfo<int>(SitecoreInfoType.Version); }
        }
        public string Name
        {
            get { return GetInfo<string>(SitecoreInfoType.Name); }
        }

        public Language Language
        {
            get { return GetInfo<Language>(SitecoreInfoType.Language); }
        }

        public IEnumerable<Guid> BaseTemplateIds
        {
            get { return GetInfo<IEnumerable<Guid>>(SitecoreInfoType.BaseTemplateIds); }
        }




    }

}




