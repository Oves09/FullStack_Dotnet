using SecureTaskManager.ViewModels;

namespace SecureTaskManager.Validators
{
    public interface ITaskValidator
    {
        bool ValidateTaskInput(TaskViewModel model, out List<string> errors);
        bool IsValidTitle(string title);
        bool IsValidDescription(string description);
    }
}