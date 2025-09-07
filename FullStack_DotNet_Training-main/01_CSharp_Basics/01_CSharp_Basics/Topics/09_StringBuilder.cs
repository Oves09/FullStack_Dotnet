using System;
using System.Text;

namespace CSharpBasics.Topics
{
    class StringBuilderExample
    {
        public static void Run()
        {
            // String
            string s = "Hello";
            s += " World"; // creates new string
            Console.WriteLine(s);

            // StringBuilder
            StringBuilder sb = new StringBuilder("Hello");
            sb.Append(" World"); // modifies in-place
            Console.WriteLine(sb.ToString());
        }
    }
}
