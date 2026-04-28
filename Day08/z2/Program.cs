using System;
using System.Collections.Generic;

// 1. Класс-коллекция (Дженерик)
class MyMultiMap<TKey, TValue>
{
    private Dictionary<TKey, List<TValue>> _data = new Dictionary<TKey, List<TValue>>();

    public void Add(TKey key, TValue value)
    {
        if (!_data.ContainsKey(key)) _data[key] = new List<TValue>();
        _data[key].Add(value);
    }

    public List<TValue> Find(TKey key)
    {
        return _data.ContainsKey(key) ? _data[key] : new List<TValue>();
    }

    public bool Remove(TKey key, TValue value)
    {
        if (_data.ContainsKey(key)) return _data[key].Remove(value);
        return false;
    }
}

// 2. Класс-контроллер
class MultiMapManager<K, V>
{
    private MyMultiMap<K, V> _map = new MyMultiMap<K, V>();

    public void AddData(K key, V val) => _map.Add(key, val);

    public void ShowByKey(K key)
    {
        var items = _map.Find(key);
        Console.WriteLine($"Ключ {key}: " + string.Join(", ", items));
    }
}