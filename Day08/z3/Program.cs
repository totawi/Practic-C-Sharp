using System;
using System.Collections.Generic;

// 1. Обобщенный интерфейс
interface IListManager<T>
{
    void Add(T item);
    void Remove(T item);
    T GetAt(int index);
    IEnumerable<T> GetAll();
}

// 2. Реализация интерфейса
class SimpleListManager<T> : IListManager<T>
{
    private List<T> _items = new List<T>();

    public void Add(T item) => _items.Add(item);
    public void Remove(T item) => _items.Remove(item);
    public T GetAt(int index) => _items[index];
    public IEnumerable<T> GetAll() => _items;
}

// 3. Класс бизнес-логики
class ListManagerLogic<T>
{
    private IListManager<T> _manager;

    public ListManagerLogic(IListManager<T> manager) => _manager = manager;

    public void PrintAll()
    {
        foreach (var item in _manager.GetAll()) Console.WriteLine(item);
    }

    public bool Contains(T item)
    {
        foreach (var x in _manager.GetAll())
        {
            if (x.Equals(item)) return true;
        }
        return false;
    }
}

class Program
{
    static void Main()
    {
        // Пример работы
        var storage = new SimpleListManager<string>();
        var logic = new ListManagerLogic<string>(storage);

        storage.Add("Первый");
        storage.Add("Второй");

        logic.PrintAll();
        Console.WriteLine("Есть 'Первый'?: " + logic.Contains("Первый"));
    }
}