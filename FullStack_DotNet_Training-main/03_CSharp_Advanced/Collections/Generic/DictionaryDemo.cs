using System;
using System.Collections.Generic;

namespace CSharpAdvanced.Collections.GenericCollections
{
    public class DictionaryDemo
    {
        public static void Demo()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict[1] = "One";
            dict[2] = "Two";

            foreach (var kv in dict)
                Console.WriteLine($"{kv.Key}: {kv.Value}");
        }
    }
}
