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

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// Class PipelineException
    /// </summary>
    public class PipelineException : ApplicationException
    {
        /// <summary>
        /// Gets the args.
        /// </summary>
        /// <value>The args.</value>
        public AbstractPipelineArgs Args { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        public PipelineException(string message, AbstractPipelineArgs args)
        {
            Args = args;
        }
    }
}




