public class ForexMarket
{
    private List<ICurrencyObserver> _observers = new List<ICurrencyObserver>();

    public void Subscribe(ICurrencyObserver observer) => _observers.Add(observer);

    public void Unsubscribe(ICurrencyObserver observer) => _observers.Remove(observer);

    public void Notify(string currency, double rate)
    {
        foreach (var observer in _observers)
        {
            observer.Update(currency, rate);
        }
    }

    public void ChangeRate(string currency, double newRate)
    {
        Console.WriteLine($"\n[Рынок]: Курс {currency} изменился до {newRate}");
        Notify(currency, newRate);
    }
}
