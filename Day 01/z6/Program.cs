Console.Write("Введите номер вагона: ");
int num = int.Parse(Console.ReadLine());

if (num >= 10 && num <= 17)
{
    Console.WriteLine("Это купейный вагон");
}
else if (num >= 1 && num < 10)
{
    Console.WriteLine("Это плацкартный вагон");
}