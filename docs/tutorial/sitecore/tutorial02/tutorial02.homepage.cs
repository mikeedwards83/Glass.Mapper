using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc.Models.Landing
{

    [SitecoreType(AutoMap = true)]
    public class HomePage
    {
        public virtual string Title { get; set; }
        public virtual string MainBody { get; set; }
    }

}