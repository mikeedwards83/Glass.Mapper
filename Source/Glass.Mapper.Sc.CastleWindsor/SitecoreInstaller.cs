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
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Sc.CastleWindsor.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.IoC;
using Sitecore.Web.UI.HtmlControls;

namespace Glass.Mapper.Sc.CastleWindsor
{
    public class WindsorSitecoreInstaller : IoC.SitecoreInstaller
    {
        public WindsorSitecoreInstaller(Sc.Config config, IWindsorContainer container)
            : this(config, new WindsorDependencyRegistrar(container))
        {
        }

        protected WindsorSitecoreInstaller(Sc.Config config, IDependencyRegistrar dependencyRegistrar)
            : base(config, dependencyRegistrar)
        {
        }

        /// <summary>
        /// The install method
        /// </summary>
        public override void Install()
        {
            Config windsorConfig = Config as Config;
            int index = ObjectionConstructionTaskInstaller.Actions.FindIndex(x => x.Key == "CreateMultiInferaceTask");
            if (windsorConfig != null && windsorConfig.UseWindsorContructor && index >= 0)
            {
               ObjectionConstructionTaskInstaller.Actions.Insert(index, new DependencyInstaller("WindsorConstruction", x => x.RegisterTransient<IObjectConstructionTask, WindsorConstruction>())); 
            }
            base.Install();
        }
    }
}
