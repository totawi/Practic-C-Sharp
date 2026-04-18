string text = "бурмалда";
char winner = ' ';
int max = 0;

for (int i = 0; i < text.Length; i++)
{
    int count = 0;
    for (int j = 0; j < text.Length; j++)
    {
        if (text[i] == text[j]) count++;
    }

    if (count > max)
    {
        max = count;
        winner = text[i];
    }
}
Console.WriteLine("Самый частый символ: " + winner);