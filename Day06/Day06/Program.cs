using System;

public delegate void OrderHandler(string orderName);

class CookOrder
{
    public void Cook(string dish) => Console.WriteLine($"Повар готовит: {dish}");
}

class DeliverOrder
{
    public void Deliver(string dish) => Console.WriteLine($"Курьер везет: {dish}");
}

class Program
{
    static void Main()
    {
        CookOrder chef = new CookOrder();
        DeliverOrder courier = new DeliverOrder();

        OrderHandler handler = chef.Cook;
        handler += courier.Deliver; 

        handler("Пицца 4 сыра");
    }
}