using System;

interface IImageFilter { void ApplyFilter(string name); }
interface IVideoFilter { void ApplyFilter(string name); }

class MediaProcessor : IImageFilter, IVideoFilter
{
    // Явная реализация для картинок
    void IImageFilter.ApplyFilter(string name)
    {
        Console.WriteLine("Применен фильтр к фото: " + name);
    }

    // Явная реализация для видео
    void IVideoFilter.ApplyFilter(string name)
    {
        Console.WriteLine("Применен фильтр к видео: " + name);
    }
}

class Program
{
    static void Main()
    {
        MediaProcessor processor = new MediaProcessor();

        // Доступ только через ссылки интерфейсов
        IImageFilter imageRef = processor;
        imageRef.ApplyFilter("Чёрно-белый");

        IVideoFilter videoRef = processor;
        videoRef.ApplyFilter("Замедление");
    }
}