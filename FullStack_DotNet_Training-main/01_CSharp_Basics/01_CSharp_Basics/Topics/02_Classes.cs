using System;

namespace CSharpBasics.Topics
{
    class ClassesExample
    {
        class Person
        {
            public string Name;
            public int Age;

            public void Display()
            {
                Console.WriteLine($"Name: {Name}, Age: {Age}");
            }
        }

        public static void Run()
        {
            Person p = new Person();
            p.Name = "Abhijeet";
            p.Age = 23;
            p.Display();
        }
    }
    
}