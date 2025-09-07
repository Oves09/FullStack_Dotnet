using System;
using System.Reflection;

namespace CSharpBasics.Topics
{
    class AssemblyExample
    {
        public static void Run()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine("Assembly Full Name: " + assembly.FullName);
        }
    }
}
