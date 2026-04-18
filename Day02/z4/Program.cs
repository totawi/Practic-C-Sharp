int[,] arr = { { 100, 200 }, { 300, 400 } };
int totalSum = 0;
foreach (int x in arr) totalSum += x;

double avg = (double)totalSum / arr.Length;
Console.WriteLine("Среднее: " + avg);

if (totalSum >= 1000 && totalSum <= 9999)
    Console.WriteLine("Сумма четырехзначная");
else
    Console.WriteLine("Сумма не четырехзначная");