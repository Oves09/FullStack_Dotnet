using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public IActionResult Index()
        {
            return View();
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            var model = new Person();
            return View(model);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                // In a real application, you would save to database
                // For this demo, we'll just pass to success page
                return View("Success", person);
            }

            // If model is invalid, return to form with validation errors
            return View(person);
        }

        // GET: Person/SimpleForm
        public IActionResult SimpleForm()
        {
            return View();
        }

        // POST: Person/SimpleForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleForm(string firstName, string lastName, int age)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || age <= 0)
            {
                ViewBag.ErrorMessage = "Please fill in all fields with valid data.";
                ViewBag.FirstName = firstName;
                ViewBag.LastName = lastName;
                ViewBag.Age = age;
                return View();
            }

            ViewBag.Message = $"Hello {firstName} {lastName}, you are {age} years old!";
            return View("SimpleSuccess");
        }
    }
}