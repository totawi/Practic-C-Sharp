int[][] step = new int[3][];
step[0] = new int[] { 1, 2 };
step[1] = new int[] { 3, 4, 5 };
step[2] = new int[] { 6 };

for (int i = 0; i < step.Length; i++)
{
    Array.Reverse(step[i]); // Зеркально переворачиваем каждую строку
    for (int j = 0; j < step[i].Length; j++)
        Console.Write(step[i][j] + " ");
    Console.WriteLine();
}