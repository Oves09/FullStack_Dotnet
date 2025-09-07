using Microsoft.EntityFrameworkCore;
using SecureTaskManager.Data;
using SecureTaskManager.Models;

namespace SecureTaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetUserTasksAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id, string userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTaskAsync(int id, string userId)
        {
            var task = await GetTaskByIdAsync(id, userId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TaskBelongsToUserAsync(int taskId, string userId)
        {
            return await _context.Tasks
                .AnyAsync(t => t.Id == taskId && t.UserId == userId);
        }
    }
}