using System;
using CSharpAdvanced.Topics;

namespace CSharpAdvanced
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("C# 7.0 Features:");
            CSharp7Features.Run();

            Console.WriteLine("\nC# 8.0 Features:");
            CSharp8Features.Run();

            Console.WriteLine("\nSearching and Sorting:");
            SearchingSorting.Run();

            Console.WriteLine("\nIndexers and Attributes:");
            IndexersAttributes.Run();
        }
    }
}
