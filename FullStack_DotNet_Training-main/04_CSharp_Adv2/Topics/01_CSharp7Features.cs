using System;
using System.Collections.Generic;

namespace CSharpAdvanced.Topics
{
    public static class CSharp7Features
    {
        // Extension method example
        public static void PrintWithStars(this string str) => Console.WriteLine($"*** {str} ***");

        public static void Run()
        {
            // Extension Method
            string message = "Hello Extensions";
            message.PrintWithStars();

            // Tuples and deconstruction
            (int id, string name) person = (101, "Abhijeet");
            var (pid, pname) = person;
            Console.WriteLine($"ID: {pid}, Name: {pname}");

            // Local Functions
            int Multiply(int a, int b) => a * b;
            Console.WriteLine("Multiply: " + Multiply(5, 6));

            // Out variables
            if (int.TryParse("123", out int num))
                Console.WriteLine("Parsed: " + num);

            // Ref locals and returns
            int[] arr = { 10, 20, 30 };
            ref int refToFirst = ref arr[0];
            refToFirst = 100;
            Console.WriteLine("Ref value: " + arr[0]);

            // Discards
            _ = 50; // ignored
            Console.WriteLine("C# 7.0 Features executed!");
        }
    }
}
