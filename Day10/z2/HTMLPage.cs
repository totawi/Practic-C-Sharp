public class HTMLPage
{
    public string Content = ""; 

    public void Render()
    {
        Console.WriteLine("--- Итоговая страница ---");
        Console.WriteLine(Content);
        Console.WriteLine("-------------------------");
    }
}
