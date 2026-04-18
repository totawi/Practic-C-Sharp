Console.Write("Введите N: ");
int N = int.Parse(Console.ReadLine());
int[,] matrix = new int[N, N];
Random rnd = new Random();

int sumSquares = 0;

for (int i = 0; i < N; i++)
{
    int rowSum = 0;
    for (int j = 0; j < N; j++)
    {
        matrix[i, j] = rnd.Next(-10, 10);
        Console.Write(matrix[i, j] + "\t");

        rowSum += matrix[i, j];
        if (matrix[i, j] > 0)
        {
            sumSquares += matrix[i, j] * matrix[i, j];
        }
    }
    Console.WriteLine("| Сумма строки: " + rowSum);
}
Console.WriteLine("Сумма квадратов положительных: " + sumSquares);