using System;

class AccessDeniedException : Exception
{
    public AccessDeniedException(string message) : base(message) { }
}

class AccessControl
{
    public void CheckAccessTime(int hour)
    {
        if (hour < 9 || hour > 18)
        {
            throw new AccessDeniedException($"Доступ запрещен. Сейчас {hour}:00, а можно только с 9:00 до 18:00.");
        }
        Console.WriteLine("Доступ разрешен. Добро пожаловать!");
    }
}

class Program
{
    static void Main()
    {
        AccessControl ac = new AccessControl();
        try
        {
            ac.CheckAccessTime(20); 
        }
        catch (AccessDeniedException ex)
        {
            Console.WriteLine("Внимание: " + ex.Message);
        }
    }
}