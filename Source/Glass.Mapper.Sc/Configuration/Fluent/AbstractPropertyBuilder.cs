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
using System.Linq.Expressions;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Class AbstractPropertyBuilder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TK">The type of the TK.</typeparam>
    public abstract class AbstractPropertyBuilder<T, TK> where TK : AbstractPropertyConfiguration, new ()
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPropertyBuilder{T, TK}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <exception cref="Glass.Mapper.MapperException">To many parameters in linq expression {0}.Formatted(ex.Body)</exception>
        public AbstractPropertyBuilder(Expression<Func<T, object>> ex)
        {
            Configuration = new TK();
            if (ex.Parameters.Count > 1)
                throw new MapperException("To many parameters in linq expression {0}".Formatted(ex.Body));
            Configuration.PropertyInfo = Mapper.Utilities.GetPropertyInfo(typeof(T), ex.Body);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public TK Configuration { get; private set; }
        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>The expression.</value>
        public Expression<Func<T, object>> Expression { get; private set; }
    }
}




