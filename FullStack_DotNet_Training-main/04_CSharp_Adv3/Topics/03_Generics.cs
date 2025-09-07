using System;

namespace FullStackAdvanced2.Topics
{
    public class GenericClass<T>
    {
        public T Data { get; set; }
        public void Show() => Console.WriteLine("Data: " + Data);
    }

    public static class GenericsExample
    {
        public static void Run()
        {
            GenericClass<int> intObj = new GenericClass<int> { Data = 100 };
            intObj.Show();

            GenericClass<string> strObj = new GenericClass<string> { Data = "Hello Generics" };
            strObj.Show();
        }
    }
}
