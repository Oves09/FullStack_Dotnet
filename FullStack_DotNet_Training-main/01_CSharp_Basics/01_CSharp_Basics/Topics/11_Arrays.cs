using System;

namespace CSharpBasics.Topics
{
    class ArraysExample
    {
        public static void Run()
        {
            int[] numbers = {1,2,3}; // Single dimension
            Console.WriteLine("Single array: " + numbers[2]);

            int[,] multi = {{1,2},{3,4}}; // Multi-dimension
            Console.WriteLine("Multi-dim: " + multi[1,1]);

            int[][] jagged = new int[2][]; // Jagged array
            jagged[0] = new int[]{1,2};
            jagged[1] = new int[]{3,4,5};
            Console.WriteLine("Jagged: " + jagged[1][2]);
        }
    }
}
