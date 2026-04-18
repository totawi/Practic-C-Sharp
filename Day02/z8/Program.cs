string word1 = "апельсин";
string word2 = "спаниель";

char[] c1 = word1.ToCharArray();
char[] c2 = word2.ToCharArray();

Array.Sort(c1);
Array.Sort(c2);

if (new string(c1) == new string(c2))
    Console.WriteLine("Да, это перестановка");
else
    Console.WriteLine("Нет");