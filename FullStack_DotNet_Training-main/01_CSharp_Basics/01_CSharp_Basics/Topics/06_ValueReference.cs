using System;

namespace CSharpBasics.Topics
{
    class ValueReferenceExample
    {
        public static void Run()
        {
            int a = 10;
            int b = a;
            b = 20;
            Console.WriteLine($"Value typr -> a={a}, b={b}");

            int[] arr1 = { 1, 2, 3, 4, 5 };
            int[] arr2 = arr1;
            arr2[0] = 100;
            Console.WriteLine($"Refrence type -> arr1[0]={arr1[0]}");
        }
    }
}