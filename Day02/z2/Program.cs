Console.Write("Введите n: ");
int n = int.Parse(Console.ReadLine());
int[] a = new int[n];
Random rnd = new Random();

for (int i = 0; i < n; i++)
{
    a[i] = rnd.Next(1, 100);
    Console.Write(a[i] + " ");
}

// Сортировка (самая простая - пузырьком)
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n - 1; j++)
    {
        if (a[j] > a[j + 1])
        {
            int temp = a[j];
            a[j] = a[j + 1];
            a[j + 1] = temp;
        }
    }
}

// Подсчет нечетных
int oddCount = 0;
for (int i = 0; i < n; i++)
{
    if (a[i] % 2 != 0) oddCount++;
}

Console.WriteLine("\nКоличество нечетных: " + oddCount);