# Razor Pages – PageModels

## Key Concepts
- **PageModel**: C# class behind Razor Pages.
- **OnGet()**: Handles GET requests (page load).
- **OnPost()**: Handles POST requests (form submit).
- **OnPostAsync()**: Async handler (DB/API).
- **Multiple Handlers**: `OnPostSaveAsync()`, `OnPostDelete()` used with `asp-page-handler`.

## Example
- `/PageModelDemo` → GET loads page.
- Buttons trigger POST with different handlers:
  - Default → `OnPost()`
  - Save → `OnPostSaveAsync()`
  - Delete → `OnPostDelete()`