using System.Text;

StringBuilder mySb = new StringBuilder("бабачай");

for (int i = 0; i < mySb.Length; i++)
{
    mySb[i] = char.ToUpper(mySb[i]);
}

Console.WriteLine(mySb.ToString());