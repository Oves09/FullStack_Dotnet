using System;
using System.Threading.Tasks;

namespace FullStackAdvanced2.Topics
{
    public static class RefOutAsyncExample
    {
        // Ref and Out
        public static void RefOutDemo(ref int a, out int b)
        {
            a += 10;
            b = 50;
        }

        // Async/Await
        public static async Task RunAsync()
        {
            int x = 5;
            int y;
            RefOutDemo(ref x, out y);
            Console.WriteLine($"Ref x: {x}, Out y: {y}");

            await AsyncDemo();
        }

        private static async Task AsyncDemo()
        {
            Console.WriteLine("Async Task started");
            await Task.Delay(500);
            Console.WriteLine("Async Task Completed");
        }
    }
}
