using Glimpse.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Glimpse
{
    public class ContextTab : ITab
    {
        public string Name
        {
            get { return "Glass.Mapper Context"; }
        }

        public object GetData(ITabContext context)
        {
            return GetContexts();
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public Type RequestContextType
        {
            get { return null; }
        }

        private object GetContexts()
        {
            var contextDatas = new List<object>();

            foreach (var context in Glass.Mapper.Context.Contexts)
            {
                var contextData = new
                {
                    ContextName = context.Value.Name,
                    Configurations = context.Value.TypeConfigurations.Select(x=>
                        new{
                            Name = x.Value.Type.FullName,
                            Properties = x.Value.Properties.Select(y=>new {
                                Name = y.PropertyInfo.Name,
                                Mapper = y.Mapper
                            }),
                            AutoMapped =x.Value.AutoMap
                        })
                };
                contextDatas.Add(contextData);
            }
            return contextDatas;
        }
    }
}
