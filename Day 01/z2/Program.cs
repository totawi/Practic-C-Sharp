Console.Write("Введите число: ");
int n = int.Parse(Console.ReadLine());

List<int> digits = new List<int>();

while (n > 0)
{
    digits.Add(n % 10); 
    n = n / 10;         
}
digits.Reverse(); 

bool isGeo = false;
if (digits.Count == 3)
{
    double a = digits[0];
    double b = digits[1];
    double c = digits[2];

    isGeo = (b * b == a * c);
}

Console.WriteLine(isGeo);