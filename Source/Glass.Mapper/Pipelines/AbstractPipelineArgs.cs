/*
   Copyright 2011 Michael Edwards
 
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
//CRE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// Abstract class that acts as the base class for all arguments passed in pipelines
    /// </summary>
    public abstract class AbstractPipelineArgs
    {
        public Context Context { get; private set; }

        public bool IsAborted { get; private set; }

        protected AbstractPipelineArgs(Context context)
        {
            Context = context;
        }

        public bool AbortPipeline ()
        {
            IsAborted = true;
            return IsAborted;
        }

       
    }
}

