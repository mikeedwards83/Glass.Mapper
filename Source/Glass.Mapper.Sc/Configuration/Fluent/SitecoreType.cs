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
using System.Linq.Expressions;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the .Net class can be loaded by Glass.Sitecore.Mapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreType<T> : ISitecoreType, ISitecoreType<T>
    {
        protected SitecoreTypeConfiguration Configuration{get;set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreType{T}"/> class.
        /// </summary>
        public SitecoreType()
        {
            Configuration = new SitecoreTypeConfiguration();
            Configuration.Type = typeof(T);
            Configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(Configuration.Type);

        }

        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> TemplateId(string id)
        {
            return TemplateId(new ID(id));
        }

        // <summary>
        /// <summary>
        /// Templates the id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> TemplateId(Guid id)
        {
            return TemplateId(new ID(id));
        }

        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> TemplateId(ID id)
        {
            Configuration.TemplateId = id;
            return this;
        }

        /// <summary>
        /// Forces Glass to do a template check and only returns an class if the item 
        /// matches the template ID.
        /// </summary>
        public SitecoreType<T> EnforceTemplate()
        {
            Configuration.EnforceTemplate = SitecoreEnforceTemplate.Template;
            return this;
        }

        /// <summary>
        /// Forces Glass to do a template check and only returns an class if the item 
        /// matches the template ID or inherits a template with the templateId
        /// </summary>
        public SitecoreType<T> EnforceTemplateAndBase()
        {
            Configuration.EnforceTemplate = SitecoreEnforceTemplate.TemplateAndBase;
            return this;
        }

        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> BranchId(string id)
        {
            return BranchId(new ID(id));
        }

        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> BranchId(Guid id)
        {
            return BranchId(new ID(id));
        }


        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> BranchId(ID id)
        {
            Configuration.BranchId = id;
            return this;
        }


        /// <summary>
        /// Codes the first.
        /// </summary>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> CodeFirst()
        {
            Configuration.CodeFirst = true;
            return this;
        }

        /// <summary>
        /// Indicates if the model can be cached.
        /// </summary>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> Cachable()
        {
            Configuration.Cachable = true;
            return this;
        }

        /// <summary>
        /// Templates the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> TemplateName(string name)
        {
            Configuration.TemplateName = name;
            return this;
        }

        /// <summary>
        /// Map item's  children  to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreChildren{`0}.</returns>
        public SitecoreChildren<T> Children(Expression<Func<T, object>> ex)
        {
            SitecoreChildren<T> builder = new SitecoreChildren<T>(ex);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }



        /// <summary>
        /// Map an item field to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreField<T> Field(Expression<Func<T, object>> ex)
        {
            SitecoreField<T> builder = new SitecoreField<T>(ex);
            Configuration.AddProperty(builder.Configuration);

            return builder;
        }

        /// <summary>
        /// Ignore a specific property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreIgnore{`0}.</returns>
        public SitecoreIgnore<T> Ignore(Expression<Func<T, object>> ex)
        {
            SitecoreIgnore<T> builder = new SitecoreIgnore<T>(ex);
            Configuration.AddProperty(builder.Configuration);

            return builder;
        }


        /// <summary>
        /// Map the item ID to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreId{`0}.</returns>
        public SitecoreId<T> Id(Expression<Func<T, object>> ex)
        {
            SitecoreId<T> builder = new SitecoreId<T>(ex);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map item information  to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreInfo{`0}.</returns>
        public SitecoreInfo<T> Info(Expression<Func<T, object>> ex)
        {
            SitecoreInfo<T> builder = new SitecoreInfo<T>(ex, Configuration);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map the item being mapped to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreInfo{`0}.</returns>
        public SitecoreItem<T> Item(Expression<Func<T, object>> ex)
        {
            SitecoreItem<T> builder = new SitecoreItem<T>(ex, Configuration);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map an item's parent  to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreParent{`0}.</returns>
        public SitecoreParent<T> Parent(Expression<Func<T, object>> ex)
        {
            SitecoreParent<T> builder = new SitecoreParent<T>(ex);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map a Sitecore Query to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreQuery{`0}.</returns>
        public SitecoreQuery<T> Query(Expression<Func<T, object>> ex)
        {
            SitecoreQuery<T> builder = new SitecoreQuery<T>(ex);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map a Sitecore item to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreNode{`0}.</returns>
        public SitecoreNode<T> Node(Expression<Func<T, object>> ex)
        {
            SitecoreNode<T> builder = new SitecoreNode<T>(ex);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map an item's linked items to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreLinked{`0}.</returns>
        public SitecoreLinked<T> Linked(Expression<Func<T, object>> ex)
        {
            SitecoreLinked<T> builder = new SitecoreLinked<T>(ex);
            Configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map item fields to a class properties
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> Fields(Action<ISitecoreClassFields<T>> fields)
        {
            fields.Invoke(this);
            return this;
        }


        /// <summary>
        /// Map multiple item information to a class properties
        /// </summary>
        /// <param name="infos">The infos.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> Infos(Action<ISitecoreClassInfos<T>> infos)
        {
            infos.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map Sitecore queries to class properties
        /// </summary>
        /// <param name="queries">The queries.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> Queries(Action<ISitecoreClassQueries<T>> queries)
        {
            queries.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map Sitecore items to a class properties
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> Nodes(Action<ISitecoreClassNodes<T>> items)
        {
            items.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map an item's linked items to a class properties
        /// </summary>
        /// <param name="links">The links.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> Links(Action<ISitecoreLinkedItems<T>> links)
        {
            links.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map an item's linked items to a class properties
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns>SitecoreType{`0}.</returns>
        public SitecoreType<T> Configure(Action<ISitecoreType<T>> config)
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
        public SitecoreType<T> Import<K>(SitecoreType<K> typeConfig)
        {
            typeConfig.Configuration.Properties
                .Where(x=> Configuration.Properties.All(y=>y.PropertyInfo.Name != x.PropertyInfo.Name))
                .ForEach(x => Configuration.AddProperty(x));

            if (typeConfig.Configuration.AutoMap)
                Config.AutoMap = true;

            return this;
        }

        /// <summary>
        /// Autoes the map.
        /// </summary>
        /// <returns></returns>
        public SitecoreType<T> AutoMap()
        {
            Config.AutoMap = true;
            return this;
        }

        /// <summary>
        /// Map an item field to a class property
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreField{`0}.</returns>
        public SitecoreDelegate<T> Delegate(Expression<Func<T, object>> ex)
        {
            SitecoreDelegate<T> builder = new SitecoreDelegate<T>(ex);
            Configuration.AddProperty(builder.Configuration);

            return builder;
        }


    #region ISitecoreClass Members

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>The config.</value>
        public SitecoreTypeConfiguration Config
        {
            get { return Configuration; }
        }

        #endregion

        
    }
    #region Interfaces

    /// <summary>
    /// Interface ISitecoreType
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreType<T> : 
        ISitecoreClassFields<T>,
        ISitecoreClassInfos<T>,
        ISitecoreClassQueries<T>,
        ISitecoreClassNodes<T>,
        ISitecoreLinkedItems<T>,
        ISitecoreClassId<T>,
        ISitecoreDelegate<T>
    {
    }

    /// <summary>
    /// Interface ISitecoreClassId
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreClassId<T>
    {
        /// <summary>
        /// Ids the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreId{`0}.</returns>
        SitecoreId<T> Id(Expression<Func<T, object>> ex);
    }
    /// <summary>
    /// Interface ISitecoreClassFields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreClassFields<T>
    {
        /// <summary>
        /// Fields the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreField{`0}.</returns>
        SitecoreField<T> Field(Expression<Func<T, object>> ex);
    }
    /// <summary>
    /// Interface ISitecoreClassInfos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreClassInfos<T>
    {
        /// <summary>
        /// Infoes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreInfo{`0}.</returns>
        SitecoreInfo<T> Info(Expression<Func<T, object>> ex);
    }
    /// <summary>
    /// Interface ISitecoreClassQueries
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreClassQueries<T>
    {
        /// <summary>
        /// Queries the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreQuery{`0}.</returns>
        SitecoreQuery<T> Query(Expression<Func<T, object>> ex);
    }
    /// <summary>
    /// Interface ISitecoreClassNodes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreClassNodes<T>
    {
        /// <summary>
        /// Nodes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreNode{`0}.</returns>
        SitecoreNode<T> Node(Expression<Func<T, object>> ex);
    }
    /// <summary>
    /// Interface ISitecoreLinkedItems
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreLinkedItems<T>
    {
        /// <summary>
        /// Linkeds the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreLinked{`0}.</returns>
        SitecoreLinked<T> Linked(Expression<Func<T, object>> ex);
    }

    /// <summary>
    /// Interface ISitecoreDelegate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISitecoreDelegate<T>
    {
        /// <summary>
        /// Delegates the responsibility for fulfilment to client code
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>SitecoreDelegate{'0}</returns>
        SitecoreDelegate<T> Delegate(Expression<Func<T, object>> ex);
    }

    #endregion
}
 



