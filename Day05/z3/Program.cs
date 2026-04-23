using System;
using System.Collections.Generic;

class OfficeEquipment { public string Name; }

interface IPrinter { void Print(); }
interface IScanner { void Scan(); }

class LaserPrinter : OfficeEquipment, IPrinter
{
    public void Print() => Console.WriteLine(Name + " печатает...");
}

class DocumentScanner : OfficeEquipment, IScanner
{
    public void Scan() => Console.WriteLine(Name + " сканирует...");
}

class Program
{
    static void Main()
    {
        OfficeEquipment[] equipment = {
            new LaserPrinter { Name = "HP-101" },
            new DocumentScanner { Name = "Epson-V39" }
        };

        // Поиск всех сканеров через интерфейс
        Console.WriteLine("Поиск сканеров:");
        foreach (var item in equipment)
        {
            if (item is IScanner scanner)
            {
                scanner.Scan();
            }
        }
    }
}