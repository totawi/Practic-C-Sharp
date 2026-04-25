using System;

public delegate double DataAnalyzer(int[] data);

class Program
{
    static void AnalyzeData(int[] numbers, DataAnalyzer analyzer)
    {
        double result = analyzer(numbers);
        Console.WriteLine("Результат анализа: " + result);
    }

    static double CalculateAverage(int[] arr)
    {
        double sum = 0;
        foreach (int x in arr) sum += x;
        return sum / arr.Length;
    }

    static double FindMaximum(int[] arr)
    {
        int max = arr[0];
        foreach (int x in arr) if (x > max) max = x;
        return max;
    }

    static void Main()
    {
        int[] myData = { 10, 20, 30, 40, 50 };

        AnalyzeData(myData, CalculateAverage);
        AnalyzeData(myData, FindMaximum);
    }
}