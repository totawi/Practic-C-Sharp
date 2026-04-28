using System;
using System.Collections.Generic;
using System.IO;

// 1. Модельная работа (Категория и Название)
class CategoryItem
{
    public string Category { get; set; }
    public string Name { get; set; }

    public CategoryItem(string category, string name)
    {
        Category = category;
        Name = name;
    }
}

// 2. Класс для чтения данных
class CategoryFileReader
{
    public List<CategoryItem> ReadItems()
    {
        List<CategoryItem> items = new List<CategoryItem>();

        // Проверяем, существует ли файл
        if (!File.Exists("file.data")) return items;

        using (StreamReader reader = new StreamReader("file.data"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // Делим строку по точке с запятой
                string[] parts = line.Split(';');
                if (parts.Length == 2)
                {
                    items.Add(new CategoryItem(parts[0], parts[1]));
                }
            }
        }
        return items;
    }
}

// 3. Класс для обработки (подсчет)
class CategoryProcessor
{
    public void CountByCategory(List<CategoryItem> items)
    {
        // Словарь: Ключ — Категория, Значение — Счетчик
        Dictionary<string, int> counts = new Dictionary<string, int>();

        foreach (var item in items)
        {
            if (counts.ContainsKey(item.Category))
                counts[item.Category]++;
            else
                counts[item.Category] = 1;
        }

        // Вывод результата
        Console.WriteLine("Статистика по категориям:");
        foreach (var pair in counts)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value} зап.");
        }
    }
}

class Program
{
    static void Main()
    {
        CategoryFileReader reader = new CategoryFileReader();
        CategoryProcessor processor = new CategoryProcessor();

        // Загружаем и считаем
        List<CategoryItem> items = reader.ReadItems();
        processor.CountByCategory(items);
    }
}