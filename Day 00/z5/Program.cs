Console.Write("Введите четырехзначное число: ");
int number = int.Parse(Console.ReadLine()); 

int first = number / 1000;          
int second = (number / 100) % 10;   
int last = number % 100;            

int result = (second * 1000) + (first * 100) + last;

Console.WriteLine($"Результат перестановки: {result}");