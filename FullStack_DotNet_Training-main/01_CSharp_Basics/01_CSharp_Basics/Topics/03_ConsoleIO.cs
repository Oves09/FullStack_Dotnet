using System;

namespace CSharpBasics.Topics
{
    class ConsoleIOExample
    {
        public static void Run()
        {
            Console.WriteLine("Enter Your Name: ");
            string name = Console.ReadLine();


            Console.WriteLine("Enter Your age:");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Hello {name}, you are {age} years old.");
        }
    }
}