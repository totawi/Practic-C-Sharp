using System;

class SecurityException : Exception
{
    public SecurityException(string message, Exception inner) : base(message, inner) { }
}

class FileSecurity
{
    public void OpenSecureFile(string path)
    {
        throw new UnauthorizedAccessException("Системный отказ: нет прав доступа к " + path);
    }
}

class FileAccessManager
{
    public void AccessFile(string path)
    {
        FileSecurity fs = new FileSecurity();
        try
        {
            fs.OpenSecureFile(path);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new SecurityException("Ошибка безопасности при работе с файлом", ex);
        }
    }
}

class Program
{
    static void Main()
    {
        FileAccessManager manager = new FileAccessManager();
        try
        {
            manager.AccessFile("secret_data.txt");
        }
        catch (SecurityException ex)
        {
            Console.WriteLine("Сообщение: " + ex.Message);
            Console.WriteLine("Внутренняя ошибка: " + ex.InnerException.Message);
            Console.WriteLine("Стек вызовов: " + ex.StackTrace);
        }
    }
}