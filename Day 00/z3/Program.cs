Console.Write("Введите значение альфа (в градусах): ");
double alfa = double.Parse(Console.ReadLine());

// градус в радианы
double a = alfa * Math.PI / 180;

double top = Math.Sin(2 * a) + Math.Sin(5 * a) - Math.Sin(3 * a);

double bottom = Math.Cos(a) - Math.Cos(3 * a) + Math.Cos(5 * a);

double z1 = top / bottom;

double z2 = Math.Tan(3 * a); 

Console.WriteLine($"z1 = {z1}");
Console.WriteLine($"z2 = {z2}");