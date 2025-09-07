using System;
using System.IO;

namespace CSharpAdvanced.FileHandling
{
    public class FileCreateWrite
    {
        /*
         * Demo: File Handling - Create & Write
         */
        public static void Demo()
        {
            string path = "example.txt";

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Hello, this is file handling in C#!");
                sw.WriteLine("File created & written successfully.");
            }

            Console.WriteLine($"File created at {Path.GetFullPath(path)}");
        }
    }
}
