using Microsoft.AspNetCore.Mvc;
using ModelBindingDemo.Models;

namespace ModelBindingDemo.Controllers
{
    public class StudentController : Controller
    {
        // Simple type binding (from query string: /Student/Details?id=1&name=Abhi)
        public IActionResult Details(int id, string name)
        {
            return Content($"Id={id}, Name={name}");
        }

        // Complex type binding (from form submission)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Student student)
        {
            if (ModelState.IsValid)
                return Content($"Registered: {student.Name}, Age={student.Age}");
            
            return View(student);
        }
    }
}
