using System;
using System.Reflection;

namespace FullStackAdvanced2.Topics
{
    public class MyClass
    {
        public int Id { get; set; } = 101;
        public string Name { get; set; } = "Abhijeet";

        public void Display() => Console.WriteLine($"{Id} - {Name}");
    }

    public static class ReflectionExample
    {
        public static void Run()
        {
            Type t = typeof(MyClass);

            Console.WriteLine("Properties:");
            foreach (var prop in t.GetProperties())
            {
                Console.WriteLine(prop.Name);
            }

            Console.WriteLine("Methods:");
            foreach (var method in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Console.WriteLine(method.Name);
            }

            // Create instance dynamically
            object obj = Activator.CreateInstance(t);
            MethodInfo display = t.GetMethod("Display");
            display.Invoke(obj, null);
        }
    }
}
