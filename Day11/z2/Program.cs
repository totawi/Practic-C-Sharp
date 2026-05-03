using System;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Проверка паттерна Декоратор ===\n");

        ILogger myLogger = new BasicLogger();

        myLogger = new TimestampDecorator(myLogger);

        myLogger = new SeverityDecorator(myLogger, "SYSTEM");

        myLogger = new UserDecorator(myLogger, "Student_BSUIR");

        string finalMessage = myLogger.Log("Выполнена лабораторная работа №14");
        Console.WriteLine(finalMessage);

        Console.ReadLine();
    }
}
}