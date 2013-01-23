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
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Indicates that the .Net class can be loaded by Glass.Sitecore.Mapper
    /// </summary>
    public class SitecoreType<T> : ISitecoreClass, ISitecoreClass<T>
    {

        List<AbstractPropertyConfiguration> _properties;
        SitecoreTypeConfiguration _configuration;


       

        public SitecoreType()
        {
            _properties = new List<AbstractPropertyConfiguration>();
            _configuration = new SitecoreTypeConfiguration();
            _configuration.Type = typeof(T);
            _configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(_configuration.Type);

            
        }
        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        public SitecoreType<T> TemplateId(Guid id)
        {
            _configuration.TemplateId = id;
            return this;
        }
        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        public SitecoreType<T> BranchId(Guid id)
        {
            _configuration.BranchId = id;
            return this;
        }

        /// <summary>
        /// Map item's  children  to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreChildren<T> Children(Expression<Func<T, object>> ex)
        {
            SitecoreChildren<T> builder = new SitecoreChildren<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        

        /// <summary>
        /// Map an item field to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreField<T> Field(Expression<Func<T, object>> ex){
            SitecoreField<T> builder = new SitecoreField<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            
            return builder;
        }

      
        /// <summary>
        /// Map the item ID to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreId<T> Id(Expression<Func<T, object>> ex)
        {
            SitecoreId<T> builder = new SitecoreId<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map item information  to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreInfo<T> Info(Expression<Func<T, object>> ex)
        {
            SitecoreInfo<T> builder = new SitecoreInfo<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map an item's parent  to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreParent<T> Parent(Expression<Func<T, object>> ex)
        {
            SitecoreParent<T> builder = new SitecoreParent<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map a Sitecore Query to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreQuery<T> Query(Expression<Func<T, object>> ex)
        {
            SitecoreQuery<T> builder = new SitecoreQuery<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

       ///  <summary>
        /// Map a Sitecore item to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreNode<T> Node(Expression<Func<T, object>> ex)
        {
            SitecoreNode<T> builder = new SitecoreNode<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map an item's linked items to a class property
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreLinked<T> Linked(Expression<Func<T, object>> ex)
        {
            SitecoreLinked<T> builder = new SitecoreLinked<T>(ex);
            _configuration.AddProperty(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Map item fields to a class properties
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreType<T> Fields(Action<ISitecoreClassFields<T>> fields)
        {
            fields.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map multiple item information to a class properties
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreType<T> Nodes(Action<ISitecoreClassInfos<T>> infos)
        {
            infos.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map Sitecore queries to class properties
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreType<T> Queries(Action<ISitecoreClassQueries<T>> queries)
        {
            queries.Invoke(this);
            return this;
        }
        /// <summary>
        /// Map Sitecore items to a class properties
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreType<T> Items(Action<ISitecoreClassNodes<T>> items)
        {
            items.Invoke(this);
            return this;
        }
        /// <summary>
        /// Map an item's linked items to a class properties
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreType<T> Links(Action<ISitecoreLinkedItems<T>> links)
        {
            links.Invoke(this);
            return this;
        }

        /// <summary>
        /// Map an item's linked items to a class properties
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public SitecoreType<T> Configure(Action<ISitecoreClass<T>> config)
        {
            config.Invoke(this);
            return this;
        }
        


    
        
       

        #region ISitecoreClass Members

        public SitecoreTypeConfiguration Config
        {
            get { return _configuration; }
        }

        #endregion

        
    }
    #region Interfaces

    public interface ISitecoreClass<T> : 
        ISitecoreClassFields<T>,
        ISitecoreClassInfos<T>,
        ISitecoreClassQueries<T>,
        ISitecoreClassNodes<T>,
        ISitecoreLinkedItems<T>,
        ISitecoreClassId<T>
    {
    }

    public interface ISitecoreClassId<T>
    {
        SitecoreId<T> Id(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreClassFields<T>
    {
        SitecoreField<T> Field(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreClassInfos<T>
    {
        SitecoreInfo<T> Info(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreClassQueries<T>
    {
        SitecoreQuery<T> Query(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreClassNodes<T>
    {
        SitecoreNode<T> Node(Expression<Func<T, object>> ex);
    }
    public interface ISitecoreLinkedItems<T>
    {
        SitecoreLinked<T> Linked(Expression<Func<T, object>> ex);
    }

    #endregion
}
 


