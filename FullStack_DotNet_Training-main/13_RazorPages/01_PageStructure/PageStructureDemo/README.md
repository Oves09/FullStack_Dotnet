# Razor Pages – Page Structure

## Key Concepts
- `@page` directive turns a Razor file into an endpoint.
- Each page has a `.cshtml` file (UI) + optional `.cshtml.cs` file (logic).
- PageModel contains methods like `OnGet`, `OnPost`.
- Cleaner structure → Each page is self-contained.

## Example
- Created `Hello.cshtml` with a heading + message.
- PageModel provided data (`Message`) via OnGet().
