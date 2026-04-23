using System;
using System.Linq; 

class Program
{
    public static int[] GetUnique(int[] input)
    {
        return input.Distinct().ToArray();
    }

    static void Main()
    {
        int[] myNumbers = { 1, 2, 2, 3, 4, 4, 5 };

        int[] result = GetUnique(myNumbers);

        Console.WriteLine(string.Join(", ", result));
    }
}