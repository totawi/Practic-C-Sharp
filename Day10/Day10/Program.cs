using System;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        FontManager managerA = FontManager.GetInstance();
        managerA.LoadFont("Arial");
        managerA.LoadFont("Times New Roman");

        FontManager managerB = FontManager.GetInstance();

        Console.WriteLine(managerB.GetFont("Arial"));

        if (ReferenceEquals(managerA, managerB))
        {
            Console.WriteLine("Успех: managerA и managerB — это один и тот же объект.");
        }
    }
}