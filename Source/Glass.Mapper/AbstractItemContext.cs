using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    /// <summary>
    /// The base class for the context loading an item from the CMS
    /// </summary>
    public abstract class AbstractItemContext
    {
        public AbstractItemContext()
        {
        }


        public Type Type { get; set; }

        public AbstractTypeConfiguration Configuration { get; set; }
        public IEnumerable<AbstractDataMapper> DataMappers { get; set; }

    }
}