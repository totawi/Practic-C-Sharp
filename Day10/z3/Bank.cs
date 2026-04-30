public class Bank : ICurrencyObserver
{
    private string _bankName;
    public Bank(string name) => _bankName = name;

    public void Update(string currency, double rate)
    {
        Console.WriteLine($"Банк '{_bankName}' обновил курс обмена в отделениях для {currency} на {rate}.");
    }
}
