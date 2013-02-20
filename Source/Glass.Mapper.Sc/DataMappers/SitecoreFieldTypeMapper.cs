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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldTypeMapper : AbstractSitecoreFieldMapper
    {

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            var item = context.Item;

            if (fieldValue.IsNullOrEmpty()) return null;

            Guid id = Guid.Empty;
            Item target;

            if (Guid.TryParse(fieldValue, out id)) {

                target = item.Database.GetItem(new ID(id), item.Language);
            }
            else
            {
                target = item.Database.GetItem(fieldValue, item.Language);
            }

            if (target == null) return null;
            return context.Service.CreateType(config.PropertyInfo.PropertyType, target, IsLazy, InferType);
        }

        public override string  SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            if (value == null)
                return string.Empty;
            else
            {
                var typeConfig = context.Service.GlassContext[value.GetType()] as SitecoreTypeConfiguration;

                var item = typeConfig.ResolveItem(value, context.Service.Database);
                if(item == null)
                    throw new NullReferenceException("Could not find item to save value {0}".Formatted(Configuration));

               return item.ID.ToString();
            }
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return context[configuration.PropertyInfo.PropertyType] != null &&
                   configuration is SitecoreFieldConfiguration;
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            var scConfig = args.PropertyConfiguration as SitecoreFieldConfiguration;

            IsLazy = (scConfig.Setting & SitecoreFieldSettings.DontLoadLazily) != SitecoreFieldSettings.DontLoadLazily;
            InferType = (scConfig.Setting & SitecoreFieldSettings.InferType) == SitecoreFieldSettings.InferType;
            base.Setup(args);
        }

        protected bool InferType { get; set; }

        protected bool IsLazy { get; set; }
    }
}



