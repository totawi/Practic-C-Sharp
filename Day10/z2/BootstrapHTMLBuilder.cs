public class BootstrapHTMLBuilder : IHTMLBuilder
{
    private HTMLPage _page = new HTMLPage();
    public void BuildHeader() => _page.Content += "<header class='navbar'>Bootstrap Header</header>\n";
    public void BuildBody() => _page.Content += "<main class='container'>Bootstrap Body</main>\n";
    public void BuildFooter() => _page.Content += "<footer class='bg-dark'>Bootstrap Footer</footer>\n";
    public HTMLPage GetResult() => _page;
}
