using System;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc
{
    public class PropertyOptions 
    {
        protected GetItemOptions Options { get; set; }
        public PropertyOptions() 
            : this(new PropertyOptions())
        {
        }

        public PropertyOptions(PropertyOptions options)
        {
            Options = new GetItemOptions();
        }

        public PropertyOptions Type(Type type)
        {
            Options.Type = type;
            return this;
        }
        public PropertyOptions AddParam<TK>(TK obj)
        {
            Options.ConstructorParameters.Add(obj);
            return this;
        }

        public PropertyOptions EnforceTemplate(SitecoreEnforceTemplate option)
        {
            Options.EnforceTemplate = option;
            return this;
        }

        public PropertyOptions IsLazy(bool value = true)
        {
            Options.IsLazy = value;
            return this;
        }

        public PropertyOptions InferType(bool value = true)
        {
            Options.InferType = value;
            return this;

        }

        public PropertyOptions DisabledVersionCount(bool value = true)
        {
            Options.VersionCount = value;
            return this;
        }

        public PropertyOptions Cache(bool value =true)
        {
            Options.Cache = value;
            return this;
        }


        public static implicit operator GetItemOptions(PropertyOptions input)
        {
            return input.Options;
        }
    }

    
}
