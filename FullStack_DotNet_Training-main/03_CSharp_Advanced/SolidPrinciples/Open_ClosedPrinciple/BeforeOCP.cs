// using System;

// class AreaCalculator
// {
//     public double CalculateArea(object shape)
//     {
//         if (shape is Rectangle r)
//             return r.Width * r.Height;
//         else if (shape is Circle c)
//             return 3.14 * c.Radius * c.Radius;

//         return 0;
//     }
// }

// class Rectangle { public double Width, Height; }
// class Circle { public double Radius; }

// class Program
// {
//     static void Main()
//     {
//         var calc = new AreaCalculator();
//         Console.WriteLine("Area of rectangle:" + calc.CalculateArea(new Rectangle { Width = 5, Height = 3 }));
//         Console.WriteLine("Area of circle:" + calc.CalculateArea(new Circle { Radius = 5 }));
//     }
// }
