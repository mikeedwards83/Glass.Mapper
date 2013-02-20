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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data.Items;
using System.Dynamic;
using Sitecore.Web.UI.WebControls;
using Sitecore.Pipelines.RenderField;

namespace Glass.Mapper.Sc.Dynamic
{
    public class DynamicItem : DynamicObject
    {
        Item _item;
       
        public DynamicItem(Item item)
        {
            _item = item;
        }

        protected virtual object GetField(string fieldName, Item item){
            return new DynamicField(fieldName, item);
        }

        protected virtual DynamicItem CreateNew(Item item)
        {
            return new DynamicItem(item);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            string name = binder.Name;


            if (_item.Fields[name] != null)
            {
                result = GetField(name, _item);
               return true;
            }

            SitecoreInfoType infoType;

            if (Enum.TryParse<SitecoreInfoType>(name, out infoType))
            {
                var mapper = new SitecoreInfoMapper();
                var config = new SitecoreInfoConfiguration();
                config.Type = infoType;

                mapper.Setup(new DataMapperResolverArgs(null, config ));
                result = mapper.MapToProperty(new SitecoreDataMappingContext(null, _item, null));
                return true;
            }

           

            switch (name)
            {
                case "Parent":
                    result = CreateNew(_item.Parent);
                    break;
                case "Children":
                    result = new DynamicCollection<DynamicItem>(_item.Children.Select(x => CreateNew(x)).ToArray());
                    break;
            }
            if (result != null) return true;
            
            throw new NotSupportedException("No field of Sitecore info matches the name {0} for item {1}".Formatted(name, _item.Paths.FullPath));

        }

       

      
    }

}



