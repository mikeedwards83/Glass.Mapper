using System;
using Glass.Mapper.Sc.Builders;

namespace Glass.Mapper.Sc
{
    public class SaveOptions : WriteOptions
    {
        public object Model { get; set; }

        public Type Type { get { return Model.GetType(); } }
        
        public SaveOptions() { }

        public SaveOptions(object model)
        {
            Model = model;
        }


        public override void Validate()
        {
            if (Model == null)
            {
                throw new NullReferenceException("No SaveItem set");
            }
        }

        public override void Copy(WriteOptions other)
        {
            var local = other as SaveOptions;
            if (local != null)
            {
                this.Model = local.Model;
            }
            base.Copy(other);
        }

    
    }

}
