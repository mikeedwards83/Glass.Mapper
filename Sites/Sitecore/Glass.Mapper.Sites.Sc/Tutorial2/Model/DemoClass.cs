using System;
using Image = Glass.Mapper.Sc.Fields.Image;

namespace Glass.Mapper.Sites.Sc.Tutorial2.Model
{
    public class DemoClass
    {
        public virtual string Title { get; set; }

        public virtual string MainContent { get; set; }

        public virtual Image Image { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual string Path { get; set; }

        public virtual string Name { get; set; }

        public virtual string TemplateName { get; set; }
    }
}