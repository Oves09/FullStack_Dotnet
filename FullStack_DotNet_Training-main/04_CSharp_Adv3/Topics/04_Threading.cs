using System;
using System.Threading;

namespace FullStackAdvanced2.Topics
{
    public static class ThreadingExample
    {
        public static void Run()
        {
            Thread t = new Thread(() =>
            {
                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine($"Thread Running {i}");
                    Thread.Sleep(200);
                }
            });
            t.Start();
            t.Join();
            Console.WriteLine("Thread Completed");
        }
    }
}
