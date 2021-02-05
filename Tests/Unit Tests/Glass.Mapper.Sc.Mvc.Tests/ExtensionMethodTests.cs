using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    public class ExtensionMethodTests
    {

        public void SyntaxCheck()
        {
            IServiceCollection collection = null;

            collection.AddGlassMapper().AddMvcContext().AddRequestContext();
        }
    }
}
