using System;
using System.Collections;

namespace CSharpAdvanced.Collections.NonGenericCollections
{
    public class ArrayListDemo
    {
        public static void Demo()
        {
            ArrayList list = new ArrayList { 1, "Hello", 3.14 };
            list.Add("World");
            foreach (var item in list) Console.WriteLine(item);
        }
    }
}
