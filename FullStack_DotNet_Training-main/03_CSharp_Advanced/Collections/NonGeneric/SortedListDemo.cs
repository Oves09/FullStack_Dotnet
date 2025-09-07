using System;
using System.Collections;

namespace CSharpAdvanced.Collections.NonGenericCollections
{
    public class SortedListDemo
    {
        public static void Demo()
        {
            SortedList sl = new SortedList();
            sl.Add(2, "Two");
            sl.Add(1, "One");
            sl.Add(3, "Three");
            //sl.Add(1, "One");  => Error

            foreach (DictionaryEntry de in sl)
                Console.WriteLine($"{de.Key} => {de.Value}");
        }
    }
}
