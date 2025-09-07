# Razor Pages Deep Dive

## Key Concepts
- Each `.cshtml` file can have a `PageModel` class in `.cshtml.cs`.
- Handlers:
  - `OnGet()` → runs for GET requests.
  - `OnPost()` → runs for POST requests (form submissions).
- Model Binding works with `[BindProperty]` to bind form fields → C# property.

## Example Learned
- Created a Hello page.
- Used `asp-for="UserName"` to bind input → `UserName` property.
- OnPost handled form submission and returned a personalized greeting.
