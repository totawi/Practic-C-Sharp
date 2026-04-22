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

            Console.WriteLine("{0,2} | {1,7:F4} | {2,7:F4}", i, x, y);

            x = x + h;
        }

        Console.ReadLine(); 
    }
}