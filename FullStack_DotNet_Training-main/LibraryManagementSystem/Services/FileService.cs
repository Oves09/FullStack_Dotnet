using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class FileService
    {
        private const string filePath = "books.json";

        public async Task SaveBooksAsync(List<Book> books)
        {
            string json = JsonSerializer.Serialize(books);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<List<Book>> LoadBooksAsync()
        {
            if (!File.Exists(filePath)) return new List<Book>();
            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }
    }
}
