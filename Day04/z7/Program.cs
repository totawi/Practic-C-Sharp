using System;

class Program
{
    // для целых чисел
    static string GetGrade(int marks)
    {
        if (marks >= 90) return "A";
        if (marks >= 80) return "B";
        if (marks >= 70) return "C";
        if (marks >= 60) return "D";
        return "F";
    }

    // для дробных чисел
    static string GetGrade(double marks)
    {
        if (marks >= 90.0) return "A";
        if (marks >= 80.0) return "B";
        if (marks >= 70.0) return "C";
        if (marks >= 60.0) return "D";
        return "F";
    }

    static void Main()
    {
        Console.WriteLine(GetGrade(85));    
        Console.WriteLine(GetGrade(89.5));  
    }
}