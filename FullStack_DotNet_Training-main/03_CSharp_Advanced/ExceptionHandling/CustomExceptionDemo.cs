using System;

namespace CSharpAdvanced.ExceptionHandling
{
    public class CustomExceptionDemo
    {
        /*
         * Demo: Custom Exception in C#
         */
        public static void Demo()
        {
            try
            {
                Withdraw(-500);
            }
            catch (NegativeAmountException ex)
            {
                Console.WriteLine("Custom Exception: " + ex.Message);
            }
        }

        static void Withdraw(int amount)
        {
            if (amount < 0)
                throw new NegativeAmountException("Amount cannot be negative!");
        }
    }

    public class NegativeAmountException : Exception
    {
        public NegativeAmountException(string msg) : base(msg) { }
    }
}
