public class BlogPost : ContentBase
{
    public virtual string MainBody { get; set; }
    public virtual DateTime Date { get; set; }
    public virtual string Author { get; set; }
}