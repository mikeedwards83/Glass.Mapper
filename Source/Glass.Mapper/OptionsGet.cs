using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    public  class GetOptions : Options
    {
        private Cache _useCache = Cache.Default;
        public virtual Type Type { get; set; }

        public LazyLoading Lazy { get; set; }


        public bool InferType { get; set; }
        public IList<ConstructorParameter> ConstructorParameters { get; set; }


        /// <summary>
        /// Indicates that if an object is cached then lazy loading should be disabled. Default is true.
        /// This option can be set when you want objects in cache to be lazy loaded. See documentation about 
        /// restrictions to this feature.
        /// </summary>            
        public static bool DisableLazyLoadingForCache { get; set; } = true; 

        public Cache Cache
        {
            get { return _useCache; }
            set
            {
                _useCache = value;
                if (value.IsEnabled() && DisableLazyLoadingForCache)
                {
                    //If caching enabled then lazy loading is disabled.
                    Lazy = LazyLoading.Disabled;
                }
            }
        }

        public GetOptions()
        {
            ConstructorParameters = new List<ConstructorParameter>();
            Lazy = LazyLoading.Enabled;
        }

        public override void Validate()
        {
            if (ConstructorParameters != null && ConstructorParameters.Count > 10)
            {
                throw new NotSupportedException("Too many parameters added. Maximum 10 constructor parameters");
            }

            if (ConstructorParameters.Any(x => x == null))
            {
                throw new NotSupportedException("Constructor parameters cannot be null");
            }

            if (Type == null)
            {
                throw new NullReferenceException("No Type defined");
            }                      
        }

        public virtual void Copy(GetOptions other)
        {
            this.Type = other.Type;
            //when copying options for properties we change the lazy so self and referenced if
            //lazy loading isn't disabled
            this.Lazy = other.Lazy == LazyLoading.OnlyReferenced ? LazyLoading.Enabled : other.Lazy;
            this.InferType = other.InferType;
            this.Cache = other.Cache;
        }



      
    }
}
