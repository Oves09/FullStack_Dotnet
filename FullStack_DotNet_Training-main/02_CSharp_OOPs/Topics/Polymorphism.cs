namespace CSharpOops.Topics
{
    public class Polymorphism
    {
        public static void Run()
        {
            Console.WriteLine("--- Polymorphism Example ---");

            // Base class reference pointing to different objects
            Shape shape;

            shape = new Circle();
            shape.Draw();   // Calls Circle's version

            shape = new Square();
            shape.Draw();   // Calls Square's version
        }
    }

    // Base class
    class Shape
    {
        public virtual void Draw()
        {
            Console.WriteLine("Drawing a shape");
        }
    }

    // Derived class Circle
    class Circle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Drawing a Circle");
        }
    }

    // Derived class Square
    class Square : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Drawing a Square");
        }
    }
}
