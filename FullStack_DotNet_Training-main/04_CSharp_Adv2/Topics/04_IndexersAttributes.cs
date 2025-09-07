using System;

namespace CSharpAdvanced.Topics
{
    // Attribute example
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class InfoAttribute : Attribute
    {
        public string Author;
        public InfoAttribute(string author) => Author = author;
    }

    [Info("Abhijeet")]
    class MyClass
    {
        private string[] data = new string[3];

        // Indexer
        public string this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }
    }

    public static class IndexersAttributes
    {
        public static void Run()
        {
            MyClass obj = new MyClass();
            obj[0] = "Hello";
            obj[1] = "World";
            Console.WriteLine(obj[0] + " " + obj[1]);

            var attr = (InfoAttribute)Attribute.GetCustomAttribute(typeof(MyClass), typeof(InfoAttribute));
            Console.WriteLine("Author: " + attr.Author);
        }
    }
}
