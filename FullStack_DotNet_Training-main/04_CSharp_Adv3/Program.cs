using System;
using FullStackAdvanced2.Topics;

namespace FullStackAdvanced2
{
    class Program
    {
        static async System.Threading.Tasks.Task Main()
        {
            Console.WriteLine("Delegates:");
            DelegatesExample.Run();

            Console.WriteLine("\nReflection:");
            ReflectionExample.Run();

            Console.WriteLine("\nGenerics:");
            GenericsExample.Run();

            Console.WriteLine("\nThreading:");
            ThreadingExample.Run();

            Console.WriteLine("\nRef, Out, Async/Await:");
            await RefOutAsyncExample.RunAsync();
        }
    }
}
