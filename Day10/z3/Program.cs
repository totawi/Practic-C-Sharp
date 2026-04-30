class Program
{
    static void Main()
    {
        ForexMarket forex = new ForexMarket();

        Trader ivan = new Trader("Иван");
        Bank centralBank = new Bank("НацБанк");

        forex.Subscribe(ivan);
        forex.Subscribe(centralBank);

        forex.ChangeRate("USD", 3.25);

        forex.Unsubscribe(ivan);

        forex.ChangeRate("EUR", 3.50);
    }
}