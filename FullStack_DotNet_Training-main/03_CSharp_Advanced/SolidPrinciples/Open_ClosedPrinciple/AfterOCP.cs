using System;

interface IShape
{
    double Area();
}

class Rectangle : IShape
{
    public double Width, Height;
    public double Area() => Width * Height;
}

class Circle : IShape
{
    public double Radius;
    public double Area() => Math.PI * Radius * Radius;
}

class AreaCalculator
{
    public double CalculateArea(IShape shape) => shape.Area();
}

class Program
{
    static void Main()
    {
        var calc = new AreaCalculator();
        Console.WriteLine("Area of rectangle:" + calc.CalculateArea(new Rectangle { Width = 5, Height = 3 }));
    }
}
