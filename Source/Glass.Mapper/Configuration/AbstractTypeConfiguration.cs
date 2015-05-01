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
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Represents the configuration for a .Net type
    /// </summary>
    [DebuggerDisplay("Type: {Type}")]
    public abstract class AbstractTypeConfiguration
    {
        private IDictionary<ConstructorInfo, Delegate> _constructorMethods;

        private AbstractPropertyConfiguration[] _properties;

        /// <summary>
        /// The type this configuration represents
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get;  set; }

        /// <summary>
        /// A list of the properties configured on a type
        /// </summary>
        /// <value>The properties.</value>
        public AbstractPropertyConfiguration[] Properties { get { return _properties; } }

        /// <summary>
        /// A list of the constructors on a type
        /// </summary>
        /// <value>The constructor methods.</value>
        public IDictionary<ConstructorInfo, Delegate> ConstructorMethods { get { return _constructorMethods; } set { _constructorMethods = value;
            DefaultConstructor = _constructorMethods.Where(x=>x.Key.GetParameters().Length == 0).Select(x=>x.Value).FirstOrDefault();
        } }

        /// <summary>
        /// This is the classes default constructor
        /// </summary>
        public Delegate DefaultConstructor { get; private set; }

        /// <summary>
        /// Indicates properties should be automatically mapped
        /// </summary>
        public bool AutoMap { get; set; }


        /// <summary>
        /// Indicates that the type is cachable
        /// </summary>
        public bool Cachable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTypeConfiguration"/> class.
        /// </summary>
        public AbstractTypeConfiguration()
        {
            _properties = new AbstractPropertyConfiguration[]{};
        }



        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="property">The property.</param>
        public virtual void AddProperty(AbstractPropertyConfiguration property)
        {
            if (property != null)
            {
                if (_properties.Any(x => x.PropertyInfo.Name == property.PropertyInfo.Name))
                {
                    throw new MapperException(
                        "You cannot have duplicate mappings for properties. Property Name: {0}  Type: {1}".Formatted(
                            property.PropertyInfo.Name, Type.Name));
                }


                _properties = _properties.Concat(new[] {property}).ToArray();
                _propertMappingExpression = CreatePropertyExpression(property);
            }
        }

        private Action<object, AbstractDataMappingContext> _propertMappingExpression = (obj, context) => { };


        protected virtual Action<object, AbstractDataMappingContext> CreatePropertyExpression(
            AbstractPropertyConfiguration property)
        {
            var next = _propertMappingExpression;

            return (obj, context) =>
                {
                    try
                    {
                        property.Mapper.MapCmsToProperty(context);
                    }
                    catch (Exception e)
                    {
                        throw new MapperException(
                            "Failed to map property {0} on {1}".Formatted(property.PropertyInfo.Name,
                                property.PropertyInfo.DeclaringType.FullName), e);
                    }
                    next(obj, context);
                };
        }


        /// <summary>
        /// Maps the properties to object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="service">The service.</param>
        /// <param name="context">The context.</param>
        public void MapPropertiesToObject( object obj, IAbstractService service, AbstractTypeCreationContext context)
        {
            try
            {
                //create properties 
                AbstractDataMappingContext dataMappingContext = service.CreateDataMappingContext(context, obj);
                _propertMappingExpression(obj, dataMappingContext);

                //var tasks = Properties.Select(x =>
                //    {
                //        var t = new Task(() => x.Mapper.MapCmsToProperty(dataMappingContext));
                //        t.Start();
                //        return t;
                //    });

                //Task.WaitAll(tasks.ToArray());

                //for(int i  = Properties.Length-1; i >= 0; i--)
                //{
                //    var prop = Properties[i];

                //    try
                //    {
                //        prop.Mapper.MapCmsToProperty(context);
                //    }
                //    catch (Exception e)
                //    {
                //        throw new MapperException(
                //            "Failed to map property {0} on {1}".Formatted(prop.PropertyInfo.Name,
                //                prop.PropertyInfo.DeclaringType.FullName), e);
                //    }

                //}
            }
            catch (Exception ex)
            {
                throw new MapperException(
                           "Failed to map properties on {0}.".Formatted(context.DataSummary()), ex);
            }
        }

        /// <summary>
        /// Called when the AutoMap property is true. Automatically maps un-specified properties.
        /// </summary>
        public void PerformAutoMap()
        {
            //we now run the auto-mapping after all the static configuration is loaded
            if (AutoMap)
            {
                //TODO: ME - probably need some binding flags.
                var properties = AutoMapProperties(Type);
                foreach (var propConfig in properties)
                {
                    AddProperty(propConfig);
                }
            }
        
        }

        /// <summary>
        /// Autoes the map properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public virtual IEnumerable<AbstractPropertyConfiguration> AutoMapProperties(Type type)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
                                    BindingFlags.FlattenHierarchy;
            IEnumerable<PropertyInfo> properties = type.GetProperties(flags);

            if (type.IsInterface)
            {
                foreach (var inter in type.GetInterfaces())
                {
                    properties = properties.Union(inter.GetProperties(flags));
                }
            }

            var propList = new List<AbstractPropertyConfiguration>();

            foreach (var property in properties)
            {
                if (Properties.All(x => x.PropertyInfo != property))
                {
                    //skipped already mapped properties
                    if(_properties.Any(x=>x.PropertyInfo.Name == property.Name))
                        continue;

                      //skip properties that are actually indexers
                    if (property.GetIndexParameters().Length > 0)
                    {
                        continue;
                    }

                    //check for an attribute
                    var propConfig = AttributeTypeLoader.ProcessProperty(property);
                    if (propConfig == null)
                    {
                        //no attribute then automap
                        propConfig = AutoMapProperty(property);
                    }

                    if (propConfig != null)
                        propList.Add(propConfig);
                }
            }

            return propList;
        }

        /// <summary>
        /// Called to map each property automatically
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual AbstractPropertyConfiguration AutoMapProperty(PropertyInfo property)
        {
            return null;
        }
    }
}





