# Razor Pages – TempData & ViewData

## Key Concepts
- **ViewData**:
  - Passes data from PageModel → Razor Page
  - Only for current request
  - Syntax: `ViewData["Key"]`

- **TempData**:
  - Passes data across requests (redirects)
  - Useful for success/error messages
  - Syntax: `TempData["Key"]`

## Example
- `/Index` → uses `ViewData` to show page message
- `/About` → uses `TempData` to show message after redirect
