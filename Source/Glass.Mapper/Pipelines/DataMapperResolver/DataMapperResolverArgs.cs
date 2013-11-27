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

namespace Glass.Mapper.Pipelines.DataMapperResolver
{
    /// <summary>
    /// Class DataMapperResolverArgs
    /// </summary>
    public class DataMapperResolverArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// The configuration of the property to load
        /// </summary>
        /// <value>The property configuration.</value>
        public Configuration.AbstractPropertyConfiguration PropertyConfiguration { get; set; }

        /// <summary>
        /// The data mapper to use when loading the property
        /// </summary>
        /// <value>The result.</value>
        public AbstractDataMapper Result { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperResolverArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        public DataMapperResolverArgs(Context context, Configuration.AbstractPropertyConfiguration configuration)
            : base(context)
        {
            PropertyConfiguration = configuration;
        }
        /// <summary>
        /// A list of all the data mappers loaded by the current context
        /// </summary>
        /// <value>The data mappers.</value>
        public IEnumerable<AbstractDataMapper> DataMappers { get; set; }

        
    }
}




