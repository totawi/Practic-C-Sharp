using System;

public delegate void NotifyHandler(string message);

class DataProcessor
{
    public event NotifyHandler ProcessingCompleted; // Событие

    public void Start()
    {
        Console.WriteLine("Начинаю обработку данных...");
        ProcessingCompleted?.Invoke("Данные успешно обработаны!");
    }
}

class ReportGenerator
{
    public void OnComplete(string msg) => Console.WriteLine("Отчет создан на основе: " + msg);
}

class Notifier
{
    public void SendAlert(string msg) => Console.WriteLine("Уведомление пользователю: " + msg);
}

class Program
{
    static void Main()
    {
        DataProcessor processor = new DataProcessor();
        ReportGenerator report = new ReportGenerator();
        Notifier alert = new Notifier();

        processor.ProcessingCompleted += report.OnComplete;
        processor.ProcessingCompleted += alert.SendAlert;

        processor.Start();
    }
}