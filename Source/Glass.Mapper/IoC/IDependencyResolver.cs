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

using Glass.Mapper.Caching;
using Glass.Mapper.Maps;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;

namespace Glass.Mapper.IoC
{
    /// <summary>
    /// Interface IDependencyResolver
    /// </summary>
    public interface IDependencyResolver
    {
        Config GetConfig();
        ICacheManager GetCacheManager();
        IConfigFactory<IDataMapperResolverTask> DataMapperResolverFactory { get; }
        IConfigFactory<AbstractDataMapper> DataMapperFactory { get; }
        IConfigFactory<IConfigurationResolverTask> ConfigurationResolverFactory { get; }
        IConfigFactory<IObjectConstructionTask> ObjectConstructionFactory { get; }
        IConfigFactory<IObjectSavingTask> ObjectSavingFactory { get; }
        IConfigFactory<IGlassMap> ConfigurationMapFactory { get; } 
    }
}




