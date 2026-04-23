using System;

class Program
{
    // Процедура для расчета дней между датами
    // ref означает, что параметры могут меняться 
    static void DaysBetweenDates(ref int d1, ref int m1, ref int y1, ref int d2, ref int m2, ref int y2, out int result)
    {
        // Создаем две даты
        DateTime date1 = new DateTime(y1, m1, d1);
        DateTime date2 = new DateTime(y2, m2, d2);

        // Вычитаем одну из другой и берем результат в днях
        result = Math.Abs((date2 - date1).Days);
    }

    static void Main()
    {
        // Пара №1
        int d1 = 1, m1 = 1, y1 = 2024;
        int d2 = 10, m2 = 1, y2 = 2024;
        DaysBetweenDates(ref d1, ref m1, ref y1, ref d2, ref m2, ref y2, out int res1);
        Console.WriteLine("Между первой парой: " + res1 + " дней");

        // Пара №2
        int d3 = 1, m3 = 3, y3 = 2024;
        int d4 = 1, m4 = 3, y4 = 2025;
        DaysBetweenDates(ref d3, ref m3, ref y3, ref d4, ref m4, ref y4, out int res2);
        Console.WriteLine("Между второй парой: " + res2 + " дней");

        // Пара №3
        int d5 = 25, m5 = 12, y5 = 2023;
        int d6 = 1, m6 = 1, y6 = 2024;
        DaysBetweenDates(ref d5, ref m5, ref y5, ref d6, ref m6, ref y6, out int res3);
        Console.WriteLine("Между третьей парой: " + res3 + " дней");
    }
}