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
using Glass.Mapper.Sites.Umb.Models.Content;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Fluent;

namespace Glass.Mapper.Sites.Umb.Models.Config
{
    public class ContentConfig
    {
        public static UmbracoFluentConfigurationLoader Load()
        {
            var loader = new UmbracoFluentConfigurationLoader();

            var eventConfig = loader.Add<Event>().AutoMap();
            eventConfig.Property(x => x.Title);
            eventConfig.Id(x => x.Id);
            eventConfig.Info(x => x.ContentTypeName).InfoType(UmbracoInfoType.ContentTypeName);

            return loader;
        }
    }
}
