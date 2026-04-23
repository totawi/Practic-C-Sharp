using System;

class Program
{
    static void Sum(in int a, in int b, out int result)
    {
        result = a + b;
    }

    static void Sum(in double a, in double b, out double result)
    {
        result = a + b;
    }

    static void Main()
    {
        int x = 5;
        int y = 10;
        Sum(in x, in y, out int resInt);
        Console.WriteLine("Сумма int: " + resInt);

        double d1 = 2.5;
        double d2 = 3.5;
        Sum(in d1, in d2, out double resDouble);
        Console.WriteLine("Сумма double: " + resDouble);
    }
}