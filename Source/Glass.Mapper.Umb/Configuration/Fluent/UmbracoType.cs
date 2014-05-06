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
using System.Linq.Expressions;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the .Net class can be loaded by Glass.Umbraco.Mapper
    /// </summary>
    public class UmbracoType<T> : IUmbracoClass, IUmbracoClass<T>
    {
        private List<AbstractPropertyConfiguration> _properties;
        private readonly UmbracoTypeConfiguration _configuration;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoType{T}"/> class.
        /// </summary>
        public UmbracoType()
        {
            _properties = new List<AbstractPropertyConfiguration>();
            _configuration = new UmbracoTypeConfiguration();
            _configuration.Type = typeof (T);
            _configuration.ConstructorMethods = Mapper.Utilities.CreateConstructorDelegates(_configuration.Type);
        }

        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public UmbracoType<T> ContentTypeAlias(string alias)
        {
            _configuration.ContentTypeAlias = alias;
            return this;
        }

        /// <summary>
        /// Codes the first.
        /// </summary>
        /// <returns></returns>
        public UmbracoType<T> CodeFirst()
        {
            _configuration.CodeFirst = true;
            return this;
        }

        /// <summary>
        /// Contents the name of the type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public UmbracoType<T> ContentTypeName(string name)
        {
            _configuration.ContentTypeName = name;
            return this;
        }

        /// <summary>
        /// Map item's  children  to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public UmbracoChildren<T> Children(Expression<Func<T, object>> ex)
        {
            var builder = new UmbracoChildren<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }
        
        /// <summary>
        /// Map an item property to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public UmbracoProperty<T> Property(Expression<Func<T, object>> ex){
            var builder = new UmbracoProperty<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            
            return builder;
        }
        
        /// <summary>
        /// Map the item ID to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public UmbracoId<T> Id(Expression<Func<T, object>> ex)
        {
            var builder = new UmbracoId<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map item information  to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public UmbracoInfo<T> Info(Expression<Func<T, object>> ex)
        {
            var builder = new UmbracoInfo<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map an item's parent  to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public UmbracoParent<T> Parent(Expression<Func<T, object>> ex)
        {
            var builder = new UmbracoParent<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map item properties to a class properties
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public UmbracoType<T> Properties(Action<IUmbracoClassFields<T>> properties)
        {
            properties.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map an item's linked items to a class properties
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public UmbracoType<T> Configure(Action<IUmbracoClass<T>> config)
        {
            config.Invoke(this);
            return this;
        }

        /// <summary>
        /// Imports the properties form another type
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="typeConfig">The type config.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public UmbracoType<T> Import<K>(UmbracoType<K> typeConfig)
        {
            typeConfig._configuration.Properties.ForEach(x => _configuration.AddProperty(x));

            if (typeConfig._configuration.AutoMap)
                Config.AutoMap = true;

            return this;
        }

        /// <summary>
        /// Autoes the map.
        /// </summary>
        /// <returns></returns>
        public UmbracoType<T> AutoMap()
        {
            Config.AutoMap = true;
            return this;
        }
        
        #region IUmbracoClass Members

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public UmbracoTypeConfiguration Config
        {
            get { return _configuration; }
        }

        #endregion
    }

    #region Interfaces

    /// <summary>
    /// IUmbracoClass
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUmbracoClass<T> : 
        IUmbracoClassFields<T>,
        IUmbracoClassInfos<T>,
        IUmbracoClassId<T>
    {
    }

    /// <summary>
    /// IUmbracoClassId
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUmbracoClassId<T>
    {
        /// <summary>
        /// Ids the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        UmbracoId<T> Id(Expression<Func<T, object>> ex);
    }

    /// <summary>
    /// IUmbracoClassFields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUmbracoClassFields<T>
    {
        /// <summary>
        /// Properties the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        UmbracoProperty<T> Property(Expression<Func<T, object>> ex);
    }

    /// <summary>
    /// IUmbracoClassInfos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUmbracoClassInfos<T>
    {
        /// <summary>
        /// Infoes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        UmbracoInfo<T> Info(Expression<Func<T, object>> ex);
    }

    #endregion
}
 



