Console.WriteLine("Введите трехзначное число: ");
int num = int.Parse(Console.ReadLine()); 

int first = num / 100;    
int last = num % 10;      

int sum = first + last;
Console.WriteLine($"Сумма первой и последней: {sum}");