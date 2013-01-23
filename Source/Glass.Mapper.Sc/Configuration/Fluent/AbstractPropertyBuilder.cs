/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    public abstract class AbstractPropertyBuilder<T, TK> where TK : AbstractPropertyConfiguration, new ()
    {

        public AbstractPropertyBuilder(Expression<Func<T, object>> ex)
        {
            Configuration = new TK();
            if (ex.Parameters.Count > 1)
                throw new MapperException("To many parameters in linq expression {0}".Formatted(ex.Body));
            Configuration.PropertyInfo = Mapper.Utilities.GetPropertyInfo(typeof(T), ex.Body);
        }

        public TK Configuration { get; private set; }
        public Expression<Func<T, object>> Expression { get; private set; }
    }
}



