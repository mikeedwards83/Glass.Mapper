using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
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

        private ConcurrentDictionary<string, AbstractPropertyConfiguration> _properties;
        private ConcurrentDictionary<string, AbstractPropertyConfiguration> _privateProperties;

        /// <summary>
        /// The type this configuration represents
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get;  set; }

        public Cache Cache { get; set; }


        public AbstractPropertyConfiguration this[string key]
        {
            get
            {
                AbstractPropertyConfiguration property;
                 _properties.TryGetValue(key, out property);
                return property;
            }
        }

        public IEnumerable<AbstractPropertyConfiguration> Properties
        {
            get { return _properties.Values; }
        }

        public IEnumerable<AbstractPropertyConfiguration> PrivateProperties
        {
            get { return _privateProperties.Values; }
        }



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
        /// Initializes a new instance of the <see cref="AbstractTypeConfiguration"/> class.
        /// </summary>
        public AbstractTypeConfiguration()
        {
            _properties = new ConcurrentDictionary<string, AbstractPropertyConfiguration>();
            _privateProperties = new ConcurrentDictionary<string, AbstractPropertyConfiguration>();
        }



        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="property">The property.</param>
        public virtual void AddProperty(AbstractPropertyConfiguration property)
        {
            if (property != null)
            {
                var key = property.PropertyInfo.Name;

                if (property.PropertyInfo.GetMethod!= null && property.PropertyInfo.GetMethod.IsPrivate)
                {
                    _privateProperties[key] = property;
                }
                else
                {
                    _properties[key] = property;
                }
            }
            
        }



        /// <summary>
        /// Maps the properties to object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="service">The service.</param>
        /// <param name="context">The context.</param>
        public void MapPrivatePropertiesToObject(object obj, IAbstractService service, AbstractTypeCreationContext context)
        {
            try
            {
                if (_privateProperties.Count != 0)
                {
                    //create properties 
                    AbstractDataMappingContext dataMappingContext = context.CreateDataMappingContext(obj);

                    foreach (var property in _privateProperties.Values)
                    {
                        var prop = property;

                        try
                        {
                            prop.Mapper.MapCmsToProperty(dataMappingContext);
                        }
                        catch (MapperStackException)
                        {
                            throw;
                        }
                        catch (Exception e)
                        {
                            throw new MapperException(
                                "Failed to map property {0} on {1}".Formatted(prop.PropertyInfo.Name,
                                    prop.PropertyInfo.DeclaringType.FullName), e);
                        }

                    }
                }
            }
            catch (MapperStackException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MapperException(
                    "Failed to map properties on {0}.".Formatted(context.DataSummary()), ex);
            }
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
                if (_properties.Count != 0)
                {
                    //create properties 
                    AbstractDataMappingContext dataMappingContext = context.CreateDataMappingContext(obj);


                    foreach (var property in _properties.Values)
                    {
                        var prop = property;

                        try
                        {
                            prop.Mapper.MapCmsToProperty(dataMappingContext);
                        }
                        catch (MapperStackException)
                        {
                            throw;
                        }
                        catch (Exception e)
                        {
                            throw new MapperException(
                                "Failed to map property {0} on {1}".Formatted(prop.PropertyInfo.Name,
                                    prop.PropertyInfo.DeclaringType.FullName), e);
                        }

                    }
                }
            }
            catch (MapperStackException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MapperException(
                           "Failed to map properties on {0}.".Formatted(context.DataSummary()), ex);
            }

            MapPrivatePropertiesToObject(obj, service, context);
        }

        /// <summary>
        /// Called when the AutoMap property is true. Automatically maps un-specified properties.
        /// </summary>
        public void PerformAutoMap()
        {
            //we now run the auto-mapping after all the static configuration is loaded
            if (AutoMap)
            {
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
                var key = property.Name;
                var currentProperty = this[key];
                
                if (currentProperty == null)
                {
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

        public virtual void GetTypeOptions(GetOptions options)
        {
            if (options.Cache == Cache.Default)
            {
                options.Cache = Cache;
            }
        }
    }
}





