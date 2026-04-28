using System;
using System.IO; // Библиотека для работы с папками и файлами

class FileManager
{
    // Метод для создания и записи текста
    public void CreateAndWrite(string path)
    {
        File.WriteAllText(path, "Учеба — это круто!");
        Console.WriteLine("Файл создан.");
    }

    // Метод для удаления (с проверкой)
    public void SafeDelete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Console.WriteLine("Файл удален успешно.");
        }
    }
}

class FileInfoProvider
{
    // Метод для вывода информации
    public void ShowInfo(string path)
    {
        FileInfo info = new FileInfo(path);
        Console.WriteLine("Размер файла: " + info.Length + " байт");
        Console.WriteLine("Дата создания: " + info.CreationTime);
    }
}

class Program
{
    static void Main()
    {
        // 1. Твои переменные (имя файла по шаблону)
        string myFile = "sharipova.ua.ii";
        FileManager fm = new FileManager();
        FileInfoProvider fp = new FileInfoProvider();

        // 2. Создаем файл и сразу читаем его в консоль
        fm.CreateAndWrite(myFile);
        string content = File.ReadAllText(myFile);
        Console.WriteLine("В файле написано: " + content);

        // 3. Выводим информацию о нем (размер и дата)
        fp.ShowInfo(myFile);

        // 4. Копируем файл
        File.Copy(myFile, "copy_sharipova.ii", true);
        Console.WriteLine("Копия файла создана.");

        // 5. Переименовываем копию в sharipova.io (пункт 6)
        // В C# Rename — это Move (Перемещение)
        File.Move("copy_sharipova.ii", "sharipova.io");
        Console.WriteLine("Файл переименован в sharipova.io");

        // 6. Выводим список всех файлов в текущей папке (пункт 10)
        string[] files = Directory.GetFiles(".");
        Console.WriteLine("Список всех файлов в папке:");
        foreach (string f in files)
        {
            Console.WriteLine("-> " + f);
        }

        // 7. Запрещаем запись в файл (пункт 11)
        File.SetAttributes(myFile, FileAttributes.ReadOnly);
        Console.WriteLine("Теперь файл защищен от записи.");

        // Пробуем записать (программа не вылетит благодаря try-catch)
        try
        {
            File.WriteAllText(myFile, "Новый текст");
        }
        catch
        {
            Console.WriteLine("Ошибка! Запись запрещена (как и просили в задании).");
        }

        // 8. Снимаем защиту и удаляем все файлы .ii (пункт 9)
        File.SetAttributes(myFile, FileAttributes.Normal);
        string[] filesToDelete = Directory.GetFiles(".", "*.ii");
        foreach (string f in filesToDelete)
        {
            File.Delete(f);
            Console.WriteLine("Файл " + f + " удален по шаблону.");
        }
    }
}