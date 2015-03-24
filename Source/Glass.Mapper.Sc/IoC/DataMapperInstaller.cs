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
using System.Collections.Generic;
using Glass.Mapper.IoC;
using Glass.Mapper.Sc.DataMappers;

namespace Glass.Mapper.Sc.IoC
{
    /// <summary>
    /// Installs the components descended from AbstractDataMapper. These are used to map data
    /// to and from the CMS.
    /// </summary>
    public class DataMapperInstaller : IDependencyInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public DataMapperInstaller(Config config)
        {
            Config = config;
            PopulateActions();
        }

        /// <summary>
        /// Gets the actions
        /// </summary>
        public List<IDependencyRegister> Actions { get; private set; }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        protected void PopulateActions()
        {
            Actions = new List<IDependencyRegister>
            {
                new DependencyRegister("SitecoreIgnoreMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreIgnoreMapper>()),
                new DependencyRegister("SitecoreChildrenCastMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreChildrenCastMapper>()),
                new DependencyRegister("SitecoreChildrenMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreChildrenMapper>()),
                new DependencyRegister("SitecoreFieldBooleanMapper",x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldBooleanMapper>()),
                new DependencyRegister("SitecoreFieldDateTimeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDateTimeMapper>()),
                new DependencyRegister("SitecoreFieldDecimalMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDecimalMapper>()),
                new DependencyRegister("SitecoreFieldDoubleMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDoubleMapper>()),
                new DependencyRegister("SitecoreFieldEnumMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldEnumMapper>()),
                new DependencyRegister("SitecoreFieldFileMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldFileMapper>()),
                new DependencyRegister("SitecoreFieldFloatMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldFloatMapper>()),
                new DependencyRegister("SitecoreFieldGuidMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldGuidMapper>()),
                new DependencyRegister("SitecoreFieldHtmlEncodingMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldHtmlEncodingMapper>()),
                new DependencyRegister("SitecoreFieldIEnumerableMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldIEnumerableMapper>()),
                new DependencyRegister("SitecoreFieldImageMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldImageMapper>()),
                new DependencyRegister("SitecoreFieldIntegerMapper",x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldIntegerMapper>()),
                new DependencyRegister("SitecoreFieldLinkMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldLinkMapper>()),
                new DependencyRegister("SitecoreFieldLongMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldLongMapper>()),
                new DependencyRegister("SitecoreFieldNameValueCollectionMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNameValueCollectionMapper>()),
                new DependencyRegister("SitecoreFieldDictionaryMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDictionaryMapper>()),
                new DependencyRegister("SitecoreFieldNullableDateTimeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableDateTimeMapper>()),
                new DependencyRegister("SitecoreFieldNullableDoubleMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableDoubleMapper>()),
                new DependencyRegister("SitecoreFieldNullableDecimalMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableDecimalMapper>()),
                new DependencyRegister("SitecoreFieldNullableFloatMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableFloatMapper>()),
                new DependencyRegister("SitecoreFieldNullableGuidMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableGuidMapper>()),
                new DependencyRegister("SitecoreFieldNullableIntMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableIntMapper>()),
                new DependencyRegister("SitecoreFieldRulesMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldRulesMapper>()),
                new DependencyRegister("SitecoreFieldStreamMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldStreamMapper>()),
                new DependencyRegister("SitecoreFieldStringMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldStringMapper>()),
                new DependencyRegister("SitecoreFieldTypeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldTypeMapper>()),
                new DependencyRegister("SitecoreIdMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreIdMapper>()),
                new DependencyRegister("SitecoreItemMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreItemMapper>()),
                new DependencyRegister("SitecoreInfoMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreInfoMapper>()),
                new DependencyRegister("SitecoreNodeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreNodeMapper>()),
                new DependencyRegister("SitecoreLinkedMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreLinkedMapper>()),
                new DependencyRegister("SitecoreParentMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreParentMapper>()),
                new DependencyRegister("SitecoreDelegateMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreDelegateMapper>()),
                new DependencyRegister("SitecoreQueryMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreQueryMapper>())
            };
        }
    }
}
