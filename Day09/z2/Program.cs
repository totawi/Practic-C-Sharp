using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Нужно для удобной сортировки

// 1. Модельный класс
class Student
{
    public string Name { get; set; }
    public int Score { get; set; }

    public Student(string name, int score)
    {
        Name = name;
        Score = score;
    }
}

// 2. Класс записи
class StudentFileWriter
{
    public void WriteSortedStudents(List<Student> students)
    {
        // Сортируем список по баллам (от большего к меньшему)
        var sortedList = students.OrderByDescending(s => s.Score).ToList();

        // Записываем в файл file.data
        using (StreamWriter writer = new StreamWriter("file.data"))
        {
            foreach (var student in sortedList)
            {
                // Записываем строку в формате: Имя - Балл
                writer.WriteLine($"{student.Name};{student.Score}");
            }
        }
        Console.WriteLine("Данные отсортированы и записаны в file.data");
    }
}

class Program
{
    static void Main()
    {
        // Создаем список студентов
        List<Student> students = new List<Student>
        {
            new Student("Иван", 85),
            new Student("Мария", 98),
            new Student("Алексей", 75)
        };

        StudentFileWriter writer = new StudentFileWriter();
        writer.WriteSortedStudents(students);
    }
}