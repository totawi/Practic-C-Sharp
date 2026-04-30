public class BasicHTMLBuilder : IHTMLBuilder
{
    private HTMLPage _page = new HTMLPage();
    public void BuildHeader() => _page.Content += "<h1>Обычный заголовок</h1>\n";
    public void BuildBody() => _page.Content += "<p>Простой текст контента.</p>\n";
    public void BuildFooter() => _page.Content += "<div>Простой футер</div>\n";
    public HTMLPage GetResult() => _page;
}
