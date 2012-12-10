using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    public abstract class AbstractTypeSavingContext
    {
        public AbstractTypeConfiguration Config { get; set; }
        public object Object { get; set; }
    }
}
