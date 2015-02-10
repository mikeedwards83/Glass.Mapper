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
using Glass.Mapper.Sc.DataMappers;

namespace Glass.Mapper.Sc.IoC
{
    /// <summary>
    /// Installs the components descended from AbstractDataMapper. These are used to map data
    /// to and from the CMS.
    /// </summary>
    public class DataMapperInstaller : IGlassInstaller
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
        public List<IDependencyInstaller> Actions { get; private set; }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        protected void PopulateActions()
        {
            Actions = new List<IDependencyInstaller>
            {
                new DependencyInstaller("SitecoreIgnoreMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreIgnoreMapper>()),
                new DependencyInstaller("SitecoreChildrenCastMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreChildrenCastMapper>()),
                new DependencyInstaller("SitecoreChildrenMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreChildrenMapper>()),
                new DependencyInstaller("SitecoreFieldBooleanMapper",x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldBooleanMapper>()),
                new DependencyInstaller("SitecoreFieldDateTimeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDateTimeMapper>()),
                new DependencyInstaller("SitecoreFieldDecimalMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDecimalMapper>()),
                new DependencyInstaller("SitecoreFieldDoubleMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDoubleMapper>()),
                new DependencyInstaller("SitecoreFieldEnumMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldEnumMapper>()),
                new DependencyInstaller("SitecoreFieldFileMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldFileMapper>()),
                new DependencyInstaller("SitecoreFieldFloatMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldFloatMapper>()),
                new DependencyInstaller("SitecoreFieldGuidMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldGuidMapper>()),
                new DependencyInstaller("SitecoreFieldHtmlEncodingMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldHtmlEncodingMapper>()),
                new DependencyInstaller("SitecoreFieldIEnumerableMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldIEnumerableMapper>()),
                new DependencyInstaller("SitecoreFieldImageMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldImageMapper>()),
                new DependencyInstaller("SitecoreFieldIntegerMapper",x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldIntegerMapper>()),
                new DependencyInstaller("SitecoreFieldLinkMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldLinkMapper>()),
                new DependencyInstaller("SitecoreFieldLongMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldLongMapper>()),
                new DependencyInstaller("SitecoreFieldNameValueCollectionMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNameValueCollectionMapper>()),
                new DependencyInstaller("SitecoreFieldDictionaryMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldDictionaryMapper>()),
                new DependencyInstaller("SitecoreFieldNullableDateTimeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableDateTimeMapper>()),
                new DependencyInstaller("SitecoreFieldNullableDoubleMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableDoubleMapper>()),
                new DependencyInstaller("SitecoreFieldNullableDecimalMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableDecimalMapper>()),
                new DependencyInstaller("SitecoreFieldNullableFloatMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableFloatMapper>()),
                new DependencyInstaller("SitecoreFieldNullableGuidMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableGuidMapper>()),
                new DependencyInstaller("SitecoreFieldNullableIntMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldNullableIntMapper>()),
                new DependencyInstaller("SitecoreFieldRulesMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldRulesMapper>()),
                new DependencyInstaller("SitecoreFieldStreamMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldStreamMapper>()),
                new DependencyInstaller("SitecoreFieldStringMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldStringMapper>()),
                new DependencyInstaller("SitecoreFieldTypeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreFieldTypeMapper>()),
                new DependencyInstaller("SitecoreIdMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreIdMapper>()),
                new DependencyInstaller("SitecoreItemMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreItemMapper>()),
                new DependencyInstaller("SitecoreInfoMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreInfoMapper>()),
                new DependencyInstaller("SitecoreNodeMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreNodeMapper>()),
                new DependencyInstaller("SitecoreLinkedMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreLinkedMapper>()),
                new DependencyInstaller("SitecoreParentMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreParentMapper>()),
                new DependencyInstaller("SitecoreDelegateMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreDelegateMapper>()),
                new DependencyInstaller("SitecoreQueryMapper", x => x.RegisterTransient<AbstractDataMapper, SitecoreQueryMapper>())
            };
        }
    }
}
