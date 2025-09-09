using Microsoft.AspNetCore.Mvc;
using FeedbackPortal.Models;

namespace FeedbackPortal.Controllers
{
    public class FeedbackController : Controller
    {
        private static List<Feedback> feedbackList = new List<Feedback>();

        public IActionResult Index() => View(feedbackList);

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedbackList.Add(feedback);
                return RedirectToAction("Index");
            }
            return View(feedback);
        }
    }
}
