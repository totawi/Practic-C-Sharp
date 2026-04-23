using System;

class Program
{
    static void ShiftLeft3(ref double A, ref double B, ref double C)
    {
        double temp = A;

        A = B;
        B = C;
        C = temp;
    }

    static void Main()
    {
        double a1 = 1.0, b1 = 2.0, c1 = 3.0;
 
        double a2 = 10.5, b2 = 20.5, c2 = 30.5;

        ShiftLeft3(ref a1, ref b1, ref c1);
        Console.WriteLine($"Set 1: {a1}, {b1}, {c1}");

        ShiftLeft3(ref a2, ref b2, ref c2);
        Console.WriteLine($"Set 2: {a2}, {b2}, {c2}");
    }
}