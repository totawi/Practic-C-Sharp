Console.Write("Собственная скорость лодки (v): ");
double v = double.Parse(Console.ReadLine());

Console.Write("Скорость течения реки (v1): ");
double v1 = double.Parse(Console.ReadLine());

Console.Write("Время движения по озеру (t1): ");
double t1 = double.Parse(Console.ReadLine());

Console.Write("Время движения против течения (t2): ");
double t2 = double.Parse(Console.ReadLine());

double s = (v * t1) + ((v - v1) * t2);

Console.WriteLine($"Общий путь S = {s} км");