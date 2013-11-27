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

namespace Glass.Mapper.Pipelines.ObjectSaving.Tasks
{
    /// <summary>
    /// Class StandardSavingTask
    /// </summary>
    public class StandardSavingTask : IObjectSavingTask
    {
        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="Glass.Mapper.Pipelines.PipelineException">No config set, can not save object</exception>
        public void Execute(ObjectSavingArgs args)
        {
            var savingContext = args.SavingContext;
            AbstractDataMappingContext dataMappingContext = args.Service.CreateDataMappingContext(savingContext);

            if(savingContext.Config == null)
                throw new PipelineException("No config set, can not save object", args);

            savingContext.Config.Properties.ForEach(x => x.Mapper.MapPropertyToCms(dataMappingContext));
        }
    }
}




