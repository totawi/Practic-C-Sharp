public interface IHTMLBuilder
{
    void BuildHeader();
    void BuildBody();
    void BuildFooter();
    HTMLPage GetResult();
}
