using System;
using System.Collections;

namespace CSharpAdvanced.Collections.NonGenericCollections
{
    public class QueueDemo
    {
        public static void Demo()
        {
            Queue q = new Queue();
            q.Enqueue("A");
            q.Enqueue("B");
            Console.WriteLine($"Dequeue: {q.Dequeue()}");
        }
    }
}
