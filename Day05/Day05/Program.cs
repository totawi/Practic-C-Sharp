using System;

// Абстрактный базовый класс
abstract class DeliveryMethod
{
    public abstract void Deliver();
}

// Три наследника
class Courier : DeliveryMethod
{
    public override void Deliver() => Console.WriteLine("Доставка курьером прямо в руки.");
}

class Pickup : DeliveryMethod
{
    public override void Deliver() => Console.WriteLine("Забор товара в пункте выдачи.");
}

class Post : DeliveryMethod
{
    public override void Deliver() => Console.WriteLine("Доставка через почтовое отделение.");
}

class Program
{
    static void Main()
    {
        // Массив базового класса
        DeliveryMethod[] methods = { new Courier(), new Pickup(), new Post() };

        // Бизнес-логика: выполнение работы
        foreach (var m in methods)
        {
            m.Deliver();
        }
    }
}