Console.Write("Введите число A: ");
double a = double.Parse(Console.ReadLine());

Console.Write("Введите степень N: ");
int n = int.Parse(Console.ReadLine());

double result = 1;

for (int i = 1; i <= n; i++)
{
    result = result * a;

    Console.WriteLine($"{a} в степени {i} = {result}");
}