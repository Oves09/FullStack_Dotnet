using System;
using CSharpAdvanced.FileHandling;
using CSharpAdvanced.Collections.GenericCollections;
using CSharpAdvanced.ExceptionHandling;
using CSharpAdvanced.Collections.NonGenericCollections;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n===== C# Advanced Topics Menu =====");
            Console.WriteLine("1. File Handling - Create & Write");
            Console.WriteLine("2. GenericCollections - List<T>");
            Console.WriteLine("3. GenericCollections - Dictionary");

            Console.WriteLine("4. NonGenericCollections - ArrayList");
            Console.WriteLine("5. NonGenericCollections - SortedList");
            Console.WriteLine("6. NonGenericCollections - HashTable");
            Console.WriteLine("7. NonGenericCollections - Stack");
            Console.WriteLine("8. NonGenericCollections - Queue");


            Console.WriteLine("9. Exception Handling - Custom Exception");
            Console.WriteLine("0. Exit");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    FileCreateWrite.Demo();
                    break;
                case "2":
                    ListDemo.Demo();
                    break;
                case "3":
                    DictionaryDemo.Demo();
                    break;
                case "4":
                    ArrayListDemo.Demo();
                    break;
                case "5":
                    SortedListDemo.Demo();
                    break;
                case "6":
                    HashTableDemo.Demo();
                    break;
                case "7":
                    StackDemo.Demo();
                    break;
                case "8":
                    QueueDemo.Demo();
                    break;
                case "9":
                    CustomExceptionDemo.Demo();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice! Try again.");
                    break;
            }
        }
    }
}
