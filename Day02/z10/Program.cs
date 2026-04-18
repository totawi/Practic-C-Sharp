using System.Text.RegularExpressions;

string time = "12:30";
bool isValid = Regex.IsMatch(time, @"^[0-2][0-9]:[0-5][0-9]$");
Console.WriteLine("Корректное время? " + isValid);