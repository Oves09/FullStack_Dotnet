using System;
using System.Collections;

namespace CSharpAdvanced.Collections.NonGenericCollections
{
    public class StackDemo
    {
        public static void Demo()
        {
            Stack stack = new Stack();
            stack.Push(10);
            stack.Push(20);
            Console.WriteLine($"Pop: {stack.Pop()}");
        }
    }
}
