using System.Numerics;

int mon = 450;
int sys = 3000;
int keyboard = 200;
int mouse = 100;
Console.WriteLine($"Цена системного блока: {sys}, цена монитора: {mon}, цена клавиатуры: {keyboard}, ыена мышки: {mouse}");

Console.WriteLine("Сколько копьютеров вы хотите приобрести? ");
int value = int.Parse(Console.ReadLine());

int total = (mon + sys + keyboard + mouse) * value;
Console.WriteLine($"{value} компьютеров обойдуться вам в {total}"); 
