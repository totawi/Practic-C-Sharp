using System;

class Program
{
    static void Main()
    {
        string text = "abc";
        // строка в аррей
        Permute(text.ToCharArray(), 0, text.Length - 1);
    }

    // left: первой буквы right: последней
    static void Permute(char[] list, int left, int right)
    {
        if (left == right)
        {
            Console.WriteLine(new string(list));
        }
        else
        {
            for (int i = left; i <= right; i++)
            {
                Swap(ref list[left], ref list[i]);

                Permute(list, left + 1, right);

                Swap(ref list[left], ref list[i]);
            }
        }
    }

    static void Swap(ref char a, ref char b)
    {
        char temp = a;
        a = b;
        b = temp;
    }
}