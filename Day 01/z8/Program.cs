Console.Write("Введите N: ");
int n = int.Parse(Console.ReadLine());
double sumFor = 0;

for (int i = 1; i <= n; i++)
{
    sumFor += 1.0 / i; 
}
Console.WriteLine("Цикл for: " + sumFor.ToString("F4"));