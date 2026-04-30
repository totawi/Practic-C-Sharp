public class Trader : ICurrencyObserver
{
    private string _name;
    public Trader(string name) => _name = name;

    public void Update(string currency, double rate)
    {
        Console.WriteLine($"Трейдер {_name} анализирует: {currency} сейчас по {rate}. Пора покупать!");
    }
}
