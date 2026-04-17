Console.Write("Введите x: ");
double x = double.Parse(Console.ReadLine());
double y;

if (x > 2)
{
    y = x * Math.Pow(x - 2, 1.0 / 3.0);
}
else if (x == 2)
{
    y = x * Math.Sin(2 * x);
}
else 
{
    y = Math.Exp(-2 * x) * Math.Cos(2 * x);
}

Console.WriteLine($"Результат y = {y}");