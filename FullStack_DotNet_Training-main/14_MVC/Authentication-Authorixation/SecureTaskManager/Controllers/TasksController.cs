using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureTaskManager.Models;
using SecureTaskManager.Services;
using SecureTaskManager.Validators;
using SecureTaskManager.ViewModels;
using System.Security.Claims;

namespace SecureTaskManager.Controllers
{
    [Authorize(Policy = "UserOrAdmin")]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IInputValidationService _validationService;
        private readonly ITaskValidator _taskValidator;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(
            ITaskService taskService,
            IInputValidationService validationService,
            ITaskValidator taskValidator,
            UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _validationService = validationService;
            _taskValidator = taskValidator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var tasks = await _taskService.GetUserTasksAsync(userId);
            
            var viewModels = tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = _validationService.EscapeHtml(t.Title),
                Description = _validationService.EscapeHtml(t.Description),
                IsCompleted = t.IsCompleted,
                Priority = t.Priority,
                CreatedAt = t.CreatedAt
            });

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TaskViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Custom validation
            if (!_taskValidator.ValidateTaskInput(model, out var errors))
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var task = new TaskItem
            {
                Title = _validationService.SanitizeHtml(model.Title),
                Description = _validationService.SanitizeHtml(model.Description),
                Priority = model.Priority,
                UserId = userId
            };

            await _taskService.CreateTaskAsync(task);
            TempData["Success"] = "Task created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var task = await _taskService.GetTaskByIdAsync(id, userId);

            if (task == null)
            {
                return NotFound();
            }

            var model = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            // Verify task ownership
            if (!await _taskService.TaskBelongsToUserAsync(id, userId))
            {
                return Forbid();
            }

            // Custom validation
            if (!_taskValidator.ValidateTaskInput(model, out var errors))
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            var existingTask = await _taskService.GetTaskByIdAsync(id, userId);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = _validationService.SanitizeHtml(model.Title);
            existingTask.Description = _validationService.SanitizeHtml(model.Description);
            existingTask.IsCompleted = model.IsCompleted;
            existingTask.Priority = model.Priority;

            if (model.IsCompleted && !existingTask.IsCompleted)
            {
                existingTask.CompletedAt = DateTime.UtcNow;
            }
            else if (!model.IsCompleted && existingTask.IsCompleted)
            {
                existingTask.CompletedAt = null;
            }

            await _taskService.UpdateTaskAsync(existingTask);
            TempData["Success"] = "Task updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var task = await _taskService.GetTaskByIdAsync(id, userId);

            if (task == null)
            {
                return NotFound();
            }

            var model = new TaskViewModel
            {
                Id = task.Id,
                Title = _validationService.EscapeHtml(task.Title),
                Description = _validationService.EscapeHtml(task.Description),
                IsCompleted = task.IsCompleted,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            // Verify task ownership before deletion
            if (!await _taskService.TaskBelongsToUserAsync(id, userId))
            {
                return Forbid();
            }

            await _taskService.DeleteTaskAsync(id, userId);
            TempData["Success"] = "Task deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var task = await _taskService.GetTaskByIdAsync(id, userId);

            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = !task.IsCompleted;
            task.CompletedAt = task.IsCompleted ? DateTime.UtcNow : null;

            await _taskService.UpdateTaskAsync(task);
            
            return Json(new { success = true, isCompleted = task.IsCompleted });
        }
    }
}