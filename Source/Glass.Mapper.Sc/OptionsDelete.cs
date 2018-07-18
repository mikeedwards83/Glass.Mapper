using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Builders;

namespace Glass.Mapper.Sc
{
    public class DeleteByModelOptions : Options
    {
        public object Model { get; set; }

        public DeleteByModelOptions() { }

        public DeleteByModelOptions(object model)
        {
            Model = model;
        }

        public override void Validate()
        {
            if (Model == null)
            {
                throw new NullReferenceException("Target property is null");
            }
        }
    }
}
