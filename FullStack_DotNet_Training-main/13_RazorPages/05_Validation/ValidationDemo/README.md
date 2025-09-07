# Razor Pages – Validation

## Key Concepts
- **Data Annotations** enforce rules: `[Required]`, `[EmailAddress]`, `[Range]`.
- **ModelState.IsValid** checks if inputs pass all validation rules.
- **Validation UI** uses `asp-validation-for` and `asp-validation-summary`.
- Client-side + server-side validation = secure + user friendly.

## Example
- `/UserForm` → registration form.
- Validation prevents submitting invalid data.
- On success, redirects to `/Success/{name}`.
