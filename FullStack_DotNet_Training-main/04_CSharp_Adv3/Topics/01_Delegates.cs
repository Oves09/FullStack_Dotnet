using System;

namespace FullStackAdvanced2.Topics
{
    public delegate int MathOperation(int a, int b); // Single cast delegate

    public static class DelegatesExample
    {
        public static void Run()
        {
            // Single-cast delegate
            MathOperation add = (x, y) => x + y;
            Console.WriteLine("Add: " + add(5, 10));

            // Multicast delegate
            Action<string> multi = s => Console.WriteLine("Hello " + s);
            multi += s => Console.WriteLine("Welcome " + s);
            multi("Abhijeet");

            // Anonymous method
            MathOperation multiply = delegate (int x, int y) { return x * y; };
            Console.WriteLine("Multiply: " + multiply(3, 4));

            // Func, Action, Predicate
            Func<int, int, int> funcAdd = (x, y) => x + y;
            Console.WriteLine("Func Add: " + funcAdd(2, 3));

            Action<string> action = s => Console.WriteLine("Action: " + s);
            action("Delegate Action");

            Predicate<int> isEven = x => x % 2 == 0;
            Console.WriteLine("Is 4 Even? " + isEven(4));
        }
    }
}
