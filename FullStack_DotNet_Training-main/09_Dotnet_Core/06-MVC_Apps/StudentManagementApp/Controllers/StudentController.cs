using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementApp.Controllers
{
    public class StudentController : Controller
    {
        // temporary in-memory data store
        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Abhijeet", Age = 22, Course = "C#" },
            new Student { Id = 2, Name = "Rahul", Age = 21, Course = "Python" }
        };

        // GET: Student
        public IActionResult Index()
        {
            return View(students);
        }

        // GET: Student/Details/1
        public IActionResult Details(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            student.Id = students.Max(s => s.Id) + 1;
            students.Add(student);
            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Edit/1
        public IActionResult Edit(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: Student/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            var existing = students.FirstOrDefault(s => s.Id == student.Id);
            if (existing == null) return NotFound();

            existing.Name = student.Name;
            existing.Age = student.Age;
            existing.Course = student.Course;

            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Delete/1
        public IActionResult Delete(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: Student/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            students.Remove(student);
            return RedirectToAction(nameof(Index));
        }
    }
}
