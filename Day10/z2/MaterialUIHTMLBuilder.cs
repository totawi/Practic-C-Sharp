public class MaterialUIHTMLBuilder : IHTMLBuilder
{
    private HTMLPage _page = new HTMLPage();
    public void BuildHeader() => _page.Content += "<AppBar>Material Title</AppBar>\n";
    public void BuildBody() => _page.Content += "<Card>Material Content</Card>\n";
    public void BuildFooter() => _page.Content += "<BottomNav>Material Footer</BottomNav>\n";
    public HTMLPage GetResult() => _page;
}
