int countFor = 0;
for (int i = 100; i <= 999; i++)
{
    int c1 = i / 100;
    int c2 = (i / 10) % 10;
    int c3 = i % 10;

    if ((c1 == c2 && c1 != c3) || (c1 == c3 && c1 != c2) || (c2 == c3 && c2 != c1))
    {
        countFor++;
    }
}
Console.WriteLine("Цикл for: " + countFor);

int countWhile = 0;
int j = 100; 
while (j <= 999)
{
    int c1 = j / 100;
    int c2 = (j / 10) % 10;
    int c3 = j % 10;

    if ((c1 == c2 && c1 != c3) || (c1 == c3 && c1 != c2) || (c2 == c3 && c2 != c1))
    {
        countWhile++;
    }
    j++; 
}
Console.WriteLine("Цикл while: " + countWhile);

int countDo = 0;
int k = 100;
do
{
    int c1 = k / 100;
    int c2 = (k / 10) % 10;
    int c3 = k % 10;

    if ((c1 == c2 && c1 != c3) || (c1 == c3 && c1 != c2) || (c2 == c3 && c2 != c1))
    {
        countDo++;
    }
    k++;
} while (k <= 999);
Console.WriteLine("Цикл do while: " + countDo);