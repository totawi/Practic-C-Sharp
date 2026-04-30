class Program
{
    static void Main()
    {
        HTMLDirector director = new HTMLDirector();

        // простая страница
        IHTMLBuilder basic = new BasicHTMLBuilder();
        director.Construct(basic);
        basic.GetResult().Render();

        // Bootstrap
        IHTMLBuilder bootstrap = new BootstrapHTMLBuilder();
        director.Construct(bootstrap);
        bootstrap.GetResult().Render();

        // Material UI
        IHTMLBuilder material = new MaterialUIHTMLBuilder();
        director.Construct(material);
        material.GetResult().Render();
    }
}