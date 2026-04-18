int[] mas = { 5, 10, 3, 7, 15, 20, 1, 2, 25, 30 };
int sum = 0;
int count = 0;

for (int i = 0; i < mas.Length; i++)
{
    if (mas[i] % 5 == 0) 
    {
        sum = sum + mas[i];
        count = count + 1;
    }
}

double result = (double)sum / count;
Console.WriteLine("Среднее арифметическое: " + result);