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

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// Abstract class that acts as the base class for all arguments passed in pipelines
    /// </summary>
    public abstract class AbstractPipelineArgs
    {
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public Context Context { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is aborted.
        /// </summary>
        /// <value><c>true</c> if this instance is aborted; otherwise, <c>false</c>.</value>
        public bool IsAborted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPipelineArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected AbstractPipelineArgs(Context context)
        {
            Parameters = new Dictionary<string, object>();
            Context = context;
        }

        /// <summary>
        /// Aborts the pipeline.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool AbortPipeline ()
        {
            IsAborted = true;
            return IsAborted;
        }

       
    }
}




