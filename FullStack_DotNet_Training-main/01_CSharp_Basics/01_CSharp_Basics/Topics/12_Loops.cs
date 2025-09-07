using System;

namespace CSharpBasics.Topics
{
    class LoopsExample
    {
        public static void Run()
        {
            int a = 10;
            if (a > 5)
                Console.WriteLine("If statement: a > 5");

            int day = 3;
            switch (day)
            {
                case 1: Console.WriteLine("Switch: Monday"); break;
                case 2: Console.WriteLine("Switch: Tuesday"); break;
                default: Console.WriteLine("Switch: Other day"); break;
            }


            for (int i = 0; i < 3; i++)
                Console.WriteLine("For loop: " + i);

            int x = 0;
            while (x < 3)
            {
                Console.WriteLine("While loop: " + x);
                x++;
            }

            int y = 0;
            do
            {
                Console.WriteLine("Do-while loop" + y);
                y++;
            } while (y < 3);
        }
    }
}