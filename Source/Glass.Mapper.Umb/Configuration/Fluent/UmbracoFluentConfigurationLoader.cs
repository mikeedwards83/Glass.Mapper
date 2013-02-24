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

using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration.Fluent
{
    public class UmbracoFluentConfigurationLoader : IConfigurationLoader
    {
        List<IUmbracoClass> _types = new List<IUmbracoClass>();

        public UmbracoType<T> Add<T>()
        {
            var config = new UmbracoType<T>();
            _types.Add(config);
            return config;
        }

        public IEnumerable<AbstractTypeConfiguration> Load()
        {
            return _types.Select(x => x.Config);
        }
    }
}



