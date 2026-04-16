Console.Write("a=");
double a = double.Parse(Console.ReadLine());

Console.Write("b=");
double b = double.Parse(Console.ReadLine());

double x = (a + b) / 2;

Console.WriteLine($"{a}/2+{b}/2={x}");