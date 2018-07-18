using System;
using Glass.Mapper.Sc.Builders;

namespace Glass.Mapper.Sc
{
    public class MoveByModelOptions : Options
    {
        public object Model { get; set; }
        public object NewParent { get; set; }

        public MoveByModelOptions() { }

        public MoveByModelOptions(object model, object newParent)
        {
            Model = model;
            NewParent = newParent;
        }

        public override void Validate()
        {
            if (Model == null)
            {
                throw new NullReferenceException("Target property is null");
            }
            if (NewParent == null)
            {
                throw new NullReferenceException("NewParent property is null");
            }
        }

      
    }

}
