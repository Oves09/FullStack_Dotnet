# Razor Pages – Data Binding

## Key Concepts
- **Model Binding**: Automatically maps query strings, forms, and route values to PageModel properties.
- **Query Parameters**: Use `[BindProperty(SupportsGet = true)]`.
- **Form Binding**: Use `[BindProperty]` with `OnPost()` method.
- **Combined Binding**: Handle both query + form inputs in one page.

## Examples
- `/QueryDemo?name=Alice&age=30` → query parameter binding
- `/FormDemo` → form submission
- `/CombinedDemo?greeting=Hello` → query + form
