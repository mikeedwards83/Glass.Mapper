using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public class ItemConfiguration : AbstractPropertyConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new ItemConfiguration();
        }
    }
}
