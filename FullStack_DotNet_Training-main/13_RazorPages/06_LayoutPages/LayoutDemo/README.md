# Razor Pages – Layout Pages

## Key Concepts
- `_Layout.cshtml` defines a **common layout** (header, footer, nav).
- `_ViewStart.cshtml` ensures all pages use the layout.
- `_ViewImports.cshtml` centralizes namespaces & TagHelpers.
- `@RenderBody()` → renders page-specific content.
- `@RenderSection("name", required:false)` → optional page-specific sections.

## Example
- `/Index` → simple page content inside layout.
- `/About` → overrides **FooterSection** with custom content.
- `/Contact` → form page inside layout.
