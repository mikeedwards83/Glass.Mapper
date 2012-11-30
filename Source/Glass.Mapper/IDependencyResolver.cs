﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public interface IDependencyResolver
    {
        T Resolve<T>(IDictionary<string, object> args = null);
        void Load(string context, IGlassConfiguration config);
    }
}
