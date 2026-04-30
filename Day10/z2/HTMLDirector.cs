public class HTMLDirector
{
    public void Construct(IHTMLBuilder builder)
    {
        builder.BuildHeader();
        builder.BuildBody();
        builder.BuildFooter();
    }
}
