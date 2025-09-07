using System;

// Simple calculator demo with a testable service.
// Business logic is separated from Program for clean design.

interface ICalculator
{
    decimal Add(decimal a, decimal b);
    decimal Subtract(decimal a, decimal b);
}

class Calculator : ICalculator
{
    public decimal Add(decimal a, decimal b) => a + b;
    public decimal Subtract(decimal a, decimal b) => a - b;
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to .NET! (IntroApp)");

        var calc = new Calculator();

        // Demonstrating minimal, testable business logic
        Console.WriteLine($"2 + 3 = {calc.Add(2, 3)}");
        Console.WriteLine($"5 - 1 = {calc.Subtract(5, 1)}");

        // Show runtime and OS info
        Console.WriteLine($"Runtime: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
        Console.WriteLine($"OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
    }
}
