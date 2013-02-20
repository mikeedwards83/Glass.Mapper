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
using System.Text;
using System.Reflection;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Represents the configuration for a property on a .Net type
    /// </summary>
    public abstract class AbstractPropertyConfiguration
    {
        public PropertyInfo PropertyInfo { get;  set; }

        public AbstractDataMapper Mapper  { get; internal set; } 

        public override string ToString()
        {
            if (PropertyInfo == null)
                return "AbstractPropertyConfiguration: Property: Null";

            return "AbstractPropertyConfiguration Property: {0} Type: {1} Assembly: {2}".Formatted(PropertyInfo.Name,
                                                                     PropertyInfo.ReflectedType.FullName,
                                                                     PropertyInfo.ReflectedType.Assembly.FullName);
        }
    }
}



