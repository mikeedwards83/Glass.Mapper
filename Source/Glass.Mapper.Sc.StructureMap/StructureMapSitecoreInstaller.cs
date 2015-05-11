///*
//   Copyright 2012 Michael Edwards
 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
 
//*/ 
////-CRE-

//using Glass.Mapper.IoC;
//using Glass.Mapper.Sc.IoC;
//using StructureMap;

//namespace Glass.Mapper.Sc.StructureMap
//{
//    /// <summary>
//    /// The windsor specific Sitecore installer
//    /// </summary>
//    public class StructureMapSitecoreInstaller : SitecoreInstaller
//    {
//        public StructureMapSitecoreInstaller(Config config, IContainer container)
//            : this(config, new DependencyResolver(container))
//        {
//        }

//        protected StructureMapSitecoreInstaller(Config config, IDependencyHandler dependencyRegistrar)
//            : base(config, dependencyRegistrar)
//        {
//        }

//        /// <summary>
//        /// The install method
//        /// </summary>
//        /*public override void Install()
//        {
//            int index = ObjectionConstructionTaskInstaller.Actions.FindIndex(x => x.Key == "CreateMultiInferaceTask");
//            if (Config != null && Config.UseIoCConstructor && index >= 0)
//            {
//               ObjectionConstructionTaskInstaller.Actions.Insert(index, new DependencyInstaller("WindsorConstruction", x => x.RegisterTransient<IObjectConstructionTask, WindsorConstruction>())); 
//            }
//            base.Install();
//        }*/
//    }
//}
