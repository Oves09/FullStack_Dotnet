using System;
using System.Collections;

namespace CSharpAdvanced.Collections.NonGenericCollections
{
    public class HashTableDemo
    {
        public static void Demo()
        {
            Hashtable ht = new Hashtable();
            ht["id"] = 101;
            ht["name"] = "Abhijeet";
            Console.WriteLine($"ID: {ht["id"]}, Name: {ht["name"]}");
        }
    }
}