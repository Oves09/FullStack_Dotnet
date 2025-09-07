namespace SecureTaskManager.Services
{
    public interface IInputValidationService
    {
        string SanitizeHtml(string input);
        bool IsValidInput(string input, string pattern);
        string EscapeHtml(string input);
        bool ContainsMaliciousContent(string input);
    }
}