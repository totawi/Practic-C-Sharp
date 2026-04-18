string s1 = "listen", s2 = "silent";
char[] c1 = s1.ToCharArray();
char[] c2 = s2.ToCharArray();
Array.Sort(c1);
Array.Sort(c2);
bool isAnagram = new string(c1) == new string(c2);
Console.WriteLine("Анаграмма? " + isAnagram);