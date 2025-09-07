using System;

namespace CSharpBasics.Topics
{
    class BoxingUnboxingExample
    {
        public static void Run()
        {
            int num = 123;
            object obj = num; // Boxing
            int n = (int)obj; // Unboxing
            Console.WriteLine($"Boxed: {obj}, Unboxed: {n}");
        }
    }
}
