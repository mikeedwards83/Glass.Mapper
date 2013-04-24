public class BlogLanding : ContentBase
{
    public virtual string MainBody { get; set; }
    public virtual IEnumerable<BlogPost> Children { get; set; }
}