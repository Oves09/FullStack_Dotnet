using SecureTaskManager.Services;
using SecureTaskManager.ViewModels;
using System.Text.RegularExpressions;

namespace SecureTaskManager.Validators
{
    public class TaskValidator : ITaskValidator
    {
        private readonly IInputValidationService _validationService;

        public TaskValidator(IInputValidationService validationService)
        {
            _validationService = validationService;
        }

        public bool ValidateTaskInput(TaskViewModel model, out List<string> errors)
        {
            errors = new List<string>();

            if (!IsValidTitle(model.Title))
            {
                errors.Add("Title contains invalid characters or is too short/long");
            }

            if (!IsValidDescription(model.Description))
            {
                errors.Add("Description contains invalid content");
            }

            if (_validationService.ContainsMaliciousContent(model.Title) || 
                _validationService.ContainsMaliciousContent(model.Description))
            {
                errors.Add("Input contains potentially malicious content");
            }

            return errors.Count == 0;
        }

        public bool IsValidTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;

            if (title.Length < 3 || title.Length > 200)
                return false;

            return Regex.IsMatch(title, @"^[a-zA-Z0-9\s\-._()]+$");
        }

        public bool IsValidDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return true; // Description is optional

            if (description.Length > 1000)
                return false;

            // Allow more characters in description but still validate
            return !_validationService.ContainsMaliciousContent(description);
        }
    }
}