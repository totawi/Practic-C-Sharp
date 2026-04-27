using System;

class OverweightLuggageException : Exception
{
    public OverweightLuggageException() : base("Вес багажа превышает норму!") { }
    public OverweightLuggageException(string message) : base(message) { }
    public OverweightLuggageException(string message, Exception inner) : base(message, inner) { }
}

class Luggage
{
    public void CheckWeight(int weight)
    {
        if (weight > 23)
        {
            throw new OverweightLuggageException($"Перевес! Ваш багаж {weight} кг, а можно только 23 кг.");
        }
        Console.WriteLine("Вес в норме, проходите.");
    }
}

class Program
{
    static void Main()
    {
        Luggage myLuggage = new Luggage();
        try
        {
            myLuggage.CheckWeight(25); 
        }
        catch (OverweightLuggageException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}