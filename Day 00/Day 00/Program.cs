// See https://aka.ms/new-console-template for more information
using System.Numerics;

Console.WriteLine("Расстояние до дачи: ");
double r = int.Parse(Console.ReadLine());
Console.WriteLine("Расход бензина (литров на 100км пробега): ");
double a = int.Parse(Console.ReadLine());
double prise = 2.67; 
Console.WriteLine($"Цена литра бензина: {prise}");
double total = (r*2) /100 * a *  prise;
Console.WriteLine($"Поездка обойдеться в: {total}");