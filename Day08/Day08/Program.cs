using System;
using System.Collections;

// 1. Модельный класс
class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ServiceType { get; set; }

    public Customer(int id, string name, string service)
    {
        Id = id; Name = name; ServiceType = service;
    }
}

// 2. Класс-менеджер
class BankQueue
{
    private Queue _queue = new Queue();

    public void AddCustomer(Customer c) => _queue.Enqueue(c);

    public void ProcessCustomer()
    {
        if (_queue.Count > 0)
        {
            Customer c = (Customer)_queue.Dequeue();
            Console.WriteLine($"Обслуживание клиента: {c.Name} (Услуга: {c.ServiceType})");
        }
        else Console.WriteLine("Очередь пуста.");
    }

    public void ShowAll()
    {
        Console.WriteLine("Текущая очередь:");
        foreach (Customer c in _queue) Console.WriteLine($"- {c.Name}");
    }
}

// В Main: BankQueue b = new BankQueue(); b.AddCustomer(new Customer(1, "Иван", "Кредит"));