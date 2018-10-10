using System;
using Glass.Mapper.Sc.Builders;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc
{
    public class CreateOptions : WriteOptions
    {
        public object Parent { get; set; }
        public Type Type { get; set; }

        public Type ParentType
        {
            get { return Parent.GetType(); }
        }

        public CreateOptions()
        {
            UpdateStatistics = true;
        }

        public override void Validate()
        {
            if (Parent == null)
            {
                throw new NullReferenceException("No Parent set");
            }
        }

        
    }

    public class CreateByNameOptions : CreateOptions
    {
        public string Name { get; set; }
        public Language Language { get; set; }

        public CreateByNameOptions() { }

        public CreateByNameOptions(string name)
        {
            Name = name;
        }
    }

    public class CreateByModelOptions : CreateOptions
    {
        public object Model { get; set; }

        public CreateByModelOptions() { }

        public CreateByModelOptions(object model)
        {
            Model = model;
        }
    }

}



