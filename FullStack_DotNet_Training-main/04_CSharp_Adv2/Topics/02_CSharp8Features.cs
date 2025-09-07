using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CSharpAdvanced.Topics
{
    public static class CSharp8Features
    {
        public static async Task Run()
        {
            // Nullable reference types
#nullable enable
            string? nullableStr = null;
            Console.WriteLine(nullableStr ?? "Default value");

            // Async streams
            await foreach (var num in GetNumbersAsync())
            {
                Console.WriteLine($"Async stream: {num}");
            }

            // Ranges and Indices
            int[] arr = { 1, 2, 3, 4, 5 };
            var sub = arr[1..4]; // 2,3,4
            Console.WriteLine("Range slice: " + string.Join(",", sub));

            // Switch Expressions
            int day = 3;
            string dayName = day switch
            {
                1 => "Mon",
                2 => "Tue",
                3 => "Wed",
                _ => "Other"
            };
            Console.WriteLine("Switch expression: " + dayName);

            Console.WriteLine("C# 8.0 Features executed!");
        }

        private static async IAsyncEnumerable<int> GetNumbersAsync()
        {
            for (int i = 1; i <= 3; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }
    }
}
