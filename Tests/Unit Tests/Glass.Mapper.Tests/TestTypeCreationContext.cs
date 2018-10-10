using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Tests
{
    public class TestTypeCreationContext : AbstractTypeCreationContext
    {
        public override bool CacheEnabled { get; }
        public override AbstractDataMappingContext CreateDataMappingContext(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
