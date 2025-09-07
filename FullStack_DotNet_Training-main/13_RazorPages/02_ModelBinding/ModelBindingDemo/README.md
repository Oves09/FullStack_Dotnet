# Model Binding in Razor Pages

## Key Concepts
- `[BindProperty(SupportsGet = true)]` allows binding from query string or route.
- `[BindProperty]` (without `SupportsGet`) binds from POST form data.
- Razor Pages automatically maps URL/query/form values into C# properties.

## Examples
1. Query string `/User?name=Abhijeet` → binds into `QueryName`.
2. Route `/User/25` → binds into `UserId`.
3. Form POST → binds input into `Email`.
