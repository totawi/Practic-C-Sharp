using System;
using System.Collections.Generic;

class Employee
{
    public string Name;       
    public string Department; 

    public Employee(string name, string dept)
    {
        Name = name;
        Department = dept;
    }
}

static class EmployeeManager
{
    public static List<Employee> FindEmployeesByDepartment(Employee[] allEmployees, string deptToFind)
    {
        List<Employee> result = new List<Employee>();

        foreach (Employee emp in allEmployees)
        {
            if (emp.Department == deptToFind)
            {
                result.Add(emp); 
            }
        }

        return result;
    }
}

class Program
{
    static void Main()
    {
        Employee[] staff = new Employee[]
        {
            new Employee("Иван", "IT"),
            new Employee("Мария", "Бухгалтерия"),
            new Employee("Петр", "IT"),
            new Employee("Анна", "Продажи")
        };

        Console.WriteLine("Ищем сотрудников отдела IT:");
        List<Employee> itEmployees = EmployeeManager.FindEmployeesByDepartment(staff, "IT");

        foreach (Employee e in itEmployees)
        {
            Console.WriteLine(e.Name);
        }
    }
}