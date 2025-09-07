using System;
using System.Collections.Generic;

namespace CSharpAdvanced.Collections.GenericCollections
{
    public class ListDemo
    {
        /*
         * Demo: Generic Collection - List<T>
         */
        public static void Demo()
        {
            List<string> fruits = new List<string> { "Apple", "Banana", "Cherry" };
            fruits.Add("Mango");
            fruits.Remove("Banana");
            fruits.Add("Pineapple");
            fruits.Add("Mango");

            Console.WriteLine("Fruits in List:");
            foreach (var fruit in fruits)
                Console.WriteLine(fruit);
        }
    }
}
