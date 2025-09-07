# Razor Pages – TagHelpers

## Key Concepts
- **Built-in TagHelpers**:
  - `asp-for` → binds inputs to PageModel properties
  - `asp-validation-for` → shows validation messages
  - `form` → automatically binds to POST methods
- **Custom TagHelpers**:
  - Example: `<bold-text>` → renders `<strong>` with `Text` property
  - Create class inheriting `TagHelper` and override `Process`

## Examples
- `/Register` → uses built-in TagHelpers for form & validation
- `/Index` → uses custom BoldTagHelper
