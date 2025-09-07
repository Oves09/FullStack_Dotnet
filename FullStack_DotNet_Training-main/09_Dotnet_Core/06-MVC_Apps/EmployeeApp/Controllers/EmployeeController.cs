using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {
        // In-memory list for demo
        private static List<Employee> employees = new List<Employee>()
        {
            new Employee { Id = 1, Name = "Rohit Patil", Position = "Developer", Salary = 60000 },
            new Employee { Id = 2, Name = "Ramesh Singh", Position = "Manager", Salary = 80000 }
        };

        // GET: Employee/Index
        public IActionResult Index()
        {
            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                emp.Id = employees.Any() ? employees.Max(e => e.Id) + 1 : 1;
                employees.Add(emp);
                return RedirectToAction("Index");
            }
            return View(emp);
        }
    }
}
