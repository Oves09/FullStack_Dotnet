using SecureTaskManager.Models;

namespace SecureTaskManager.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetUserTasksAsync(string userId);
        Task<TaskItem?> GetTaskByIdAsync(int id, string userId);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<TaskItem> UpdateTaskAsync(TaskItem task);
        Task DeleteTaskAsync(int id, string userId);
        Task<bool> TaskBelongsToUserAsync(int taskId, string userId);
    }
}