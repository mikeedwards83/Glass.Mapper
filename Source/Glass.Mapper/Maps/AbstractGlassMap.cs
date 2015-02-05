using System;
using System.Collections.Generic;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Maps
{
    public abstract class AbstractGlassMap<T> : IGlassMap where T : class
    {
        private readonly List<Type> importedTypes = new List<Type>();

        private T glassType;

        protected AbstractGlassMap(IDependencyResolver dependencyResolver)
        {
            Actions = new List<Action<T>>();
            ImportActions = new List<Action<T>>();
            DependencyResolver = dependencyResolver;
        } 

        /// <summary>
        /// Gets the underlying Glass Type
        /// </summary>
        public T GlassType
        {
            get
            {
                PopulateType();
                return glassType;
            }
        }

        /// <summary>
        /// Gets the dependency resolver
        /// </summary>
        protected IDependencyResolver DependencyResolver { get; private set; }

        /// <summary>
        /// Gets the list of actions to be performed
        /// </summary>
        protected List<Action<T>> Actions { get; private set; }

        /// <summary>
        /// Gets the actions.
        /// </summary>
        protected List<Action<T>> ImportActions { get; private set; }

        /// <summary>
        /// Configures the mapping
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// Performs the map against the container
        /// </summary>
        /// <param name="mappingContainer"></param>
        public abstract void PerformMap<TLoader>(TLoader mappingContainer) where TLoader : class, IConfigurationLoader;

        /// <summary>
        /// Sets up the mapping for the object
        /// </summary>
        /// <param name="mappings"></param>
        public void Map(params Action<T>[] mappings)
        {
            if (mappings == null)
            {
                return;
            }

            Actions.AddRange(mappings);
        }


        protected bool CanLoad<TLoader>(TLoader mappingContainer) where TLoader : class
        {
            return mappingContainer == null || GlassType == null;
        }

        /// <summary>
        /// Imports a type from the configuration
        /// </summary>
        /// <typeparam name="TImport"></typeparam>
        protected virtual void ImportType<TImport>() where TImport : class
        {
            Type typeToImport = typeof(TImport);

            if (importedTypes.Contains(typeToImport))
            {
                return;
            }

            importedTypes.Add(typeToImport);
            ImportActions.Add(x => ImportMap<TImport>());
        }

        /// <summary>
        /// Imports another map
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        protected abstract void ImportMap<TK>() where TK : class;

        /// <summary>
        /// Populates the type
        /// </summary>
        protected virtual void PopulateType()
        {
            if (glassType != null)
            {
                return;
            }

            if (!HasActions())
            {
                Configure();
                if (!HasActions())
                {
                    return;
                }
            }

            glassType = CreateGlassType();
            foreach (var action in Actions)
            {
                action(glassType);
            }

            if (ImportActions == null || ImportActions.Count <= 0)
            {
                return;
            }

            foreach (var importAction in ImportActions)
            {
                importAction(this.glassType);
            }
        }

        protected abstract T CreateGlassType();

        protected bool HasActions()
        {
            return Actions != null && Actions.Count > 0;
        }
    }
}
