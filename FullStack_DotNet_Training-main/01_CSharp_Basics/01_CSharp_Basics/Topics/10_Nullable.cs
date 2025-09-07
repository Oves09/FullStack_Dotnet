using System;

namespace CSharpBasics.Topics
{
    class NullableExample
    {
        public static void Run()
        {
            int? num = 2;
            if (num.HasValue)
                Console.WriteLine(num.Value);
            else
                Console.WriteLine("Value is null");
        }
    }
}
