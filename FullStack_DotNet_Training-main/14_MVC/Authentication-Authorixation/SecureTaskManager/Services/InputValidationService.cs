using Ganss.Xss;
using System.Text.RegularExpressions;
using System.Web;

namespace SecureTaskManager.Services
{
    public class InputValidationService : IInputValidationService
    {
        private readonly HtmlSanitizer _sanitizer;
        private readonly List<string> _maliciousPatterns;

        public InputValidationService()
        {
            _sanitizer = new HtmlSanitizer();
            _sanitizer.AllowedTags.Clear();
            _sanitizer.AllowedAttributes.Clear();
            
            _maliciousPatterns = new List<string>
            {
                @"<script.*?>.*?</script>",
                @"javascript:",
                @"vbscript:",
                @"onload\s*=",
                @"onerror\s*=",
                @"onclick\s*=",
                @"<iframe.*?>.*?</iframe>",
                @"<object.*?>.*?</object>",
                @"<embed.*?>.*?</embed>",
                @"<form.*?>.*?</form>",
                @"union\s+select",
                @"drop\s+table",
                @"insert\s+into",
                @"delete\s+from",
                @"update\s+.*\s+set"
            };
        }

        public string SanitizeHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return _sanitizer.Sanitize(input);
        }

        public bool IsValidInput(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
                return false;

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public string EscapeHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return HttpUtility.HtmlEncode(input);
        }

        public bool ContainsMaliciousContent(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return _maliciousPatterns.Any(pattern => 
                Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase));
        }
    }
}