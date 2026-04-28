using System;
using System.IO;

class FileWatcher
{
    private FileSystemWatcher _watcher;
    private readonly string _logPath = "log.csv";

    public FileWatcher(string folderPath)
    {
        // Проверяем, существует ли папка
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        _watcher = new FileSystemWatcher(folderPath);

        // Указываем, на что именно реагировать
        _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

        // Подписываемся на события
        _watcher.Created += (s, e) => LogToCsv("Создан", e.Name);
        _watcher.Deleted += (s, e) => LogToCsv("Удален", e.Name);
        _watcher.Changed += (s, e) => LogToCsv("Изменен", e.Name);
        _watcher.Renamed += (s, e) => LogToCsv("Переименован", e.OldName + " -> " + e.Name);

        // Включаем слежку
        _watcher.EnableRaisingEvents = true;

        Console.WriteLine($"Слежка запущена за папкой: {folderPath}");
        Console.WriteLine("События записываются в log.csv. Нажмите Enter для выхода.");
    }

    // Метод для записи в CSV (имитация интеграции с БД)
    private void LogToCsv(string eventType, string fileName)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string line = $"{timestamp};{eventType};{fileName}{Environment.NewLine}";

        try
        {
            // Дозаписываем строку в файл (AppendAllText сам открывает и закрывает файл)
            File.AppendAllText(_logPath, line);
            Console.WriteLine($"[LOG]: {eventType} - {fileName}");
        }
        catch (IOException)
        {
            // Файл может быть занят другим процессом (например, Excel)
            Console.WriteLine("Ошибка записи в CSV: файл занят.");
        }
    }
}

class Program
{
    static void Main()
    {
        // Указываем путь к папке (например, "temp")
        string path = "MyWatchedFolder";

        FileWatcher watcher = new FileWatcher(path);

        Console.ReadLine(); // Чтобы программа не закрылась сразу
    }
}