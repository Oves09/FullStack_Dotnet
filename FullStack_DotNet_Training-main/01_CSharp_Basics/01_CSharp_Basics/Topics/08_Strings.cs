using System;

namespace CSharpBasics.Topics
{
    class StringsExample
    {
        public static void Run()
        {
            string str = "Hello";
            string name = "Abhijeet";
            string message = str + " " + name;
            Console.WriteLine(message);
            Console.WriteLine("Length: " + message.Length);
            Console.WriteLine("Uppercase: " + message.ToUpper());
        }
    }
}
