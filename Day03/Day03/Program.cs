using System;
class A
{
    public int a;
    public int b;

    public A(int A, int B)
    {
        this.a = A;
        this.b = B;
    }

    public double CalculateFirst()
    {
        double result = (4.0 / (a + 2)) * b;
        return result;
    }

    public double CalculateSecond()
    {
        return Math.Pow(b, 10);
    }
}

class Program
{
    static void Main()
    {
        A myObject = new A(2, 3);

        Console.WriteLine("Значение a: " + myObject.a);
        Console.WriteLine("Значение b: " + myObject.b);

        Console.WriteLine("Результат первого метода: " + myObject.CalculateFirst());
        Console.WriteLine("Результат второго метода (b в 10 степени): " + myObject.CalculateSecond());
    }
}