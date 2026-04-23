using System;

public static class StrHelper
{
    public static string Hide(this string s)
    {
        char[] v = { 'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y' };

        foreach (char x in v)
        {
            s = s.Replace(x, '*');
        }
        return s;
    }
}

class Program
{
    static void Main()
    {
        string text = "Hello Apple";

        Console.WriteLine(text.Hide());
    }
}