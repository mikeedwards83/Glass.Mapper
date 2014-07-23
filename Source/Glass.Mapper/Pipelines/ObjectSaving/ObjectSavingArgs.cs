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

namespace Glass.Mapper.Pipelines.ObjectSaving
{
    /// <summary>
    /// Class ObjectSavingArgs
    /// </summary>
    public class ObjectSavingArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public object Target { get; private set; }
        /// <summary>
        /// Gets the saving context.
        /// </summary>
        /// <value>The saving context.</value>
        public AbstractTypeSavingContext SavingContext { get; private set; }
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public IAbstractService Service { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSavingArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <param name="savingContext">The saving context.</param>
        /// <param name="service">The service.</param>
        public ObjectSavingArgs(
            Context context, 
            object target, 
            AbstractTypeSavingContext savingContext,
            IAbstractService service)
            : base(context)
        {
            Target = target;
            SavingContext = savingContext;
            Service = service;
        }
    }
}




