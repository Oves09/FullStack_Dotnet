
using System;

namespace CSharpBasics.Topics
{
    class PrivateSharedExample
    {
        class Demo
        {
            private string privateField = "I am private";
            public static string sharedField = "I am shared (static)";

            public void ShowPrivate()
            {
                Console.WriteLine(privateField);
            }
        }
        public static void Run()
        {
            Demo d = new Demo();
            d.ShowPrivate();
            Console.WriteLine(Demo.sharedField);
        }
    }
}