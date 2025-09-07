using System;

class Employee
{
    public string? Name { get; set; }
}

class EmployeeRepository
{
    public void Save(Employee emp)
    {
        Console.WriteLine("Saving employee to database");
    }
}

class ReportGenerator
{
    public void Generate(Employee emp)
    {
        Console.WriteLine("Generating employee report");
    }
}

class Program
{
    static void Main()
    {
        Employee emp = new Employee();
        new EmployeeRepository().Save(emp);
        new ReportGenerator().Generate(emp);
    }
}
