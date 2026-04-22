using System;

class Program
{
    static void Main()
    {
        double a = Math.PI / 3;     
        double b = 3 * Math.PI / 2; 
        int m = 15;                 

        double h = (b - a) / (m - 1);

        double x = a;

        Console.WriteLine(" i |    x    |    y");
        Console.WriteLine("---------------------");
        
        for (int i = 1; i <= m; i++)
        {
            double y = Math.Cos(x * x);

            Console.WriteLine($"{i,2} | {x,7:f4} | {y,7:f4}");

            x = x + h;
        }

        Console.ReadKey(); 
    }
}