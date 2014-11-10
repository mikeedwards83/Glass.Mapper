using System;

namespace Glass.Mapper.Configuration
{
    public class DelegatePropertyConfiguration<T> : AbstractPropertyConfiguration where T : AbstractDataMappingContext
    {
        public Action<T> MapToCmsAction { get; set; }

        public Func<T, object> MapToPropertyAction { get; set; }
    }
}