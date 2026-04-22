Console.Write("Введите трехзначное число: ");
int n = int.Parse(Console.ReadLine());

int first = n / 100;
int second = (n / 10) % 10;

int maxDigit = Math.Max(first, second);

Console.WriteLine($"Из цифр {first} и {second} самая большая: {maxDigit}");