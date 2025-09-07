# Razor Pages – Routing

## Key Concepts
- **Default Routing**: `/About.cshtml` → `/About`
- **Custom Routes**: Use `@page "/info"` → `/info`
- **Route Parameters**: `@page "/User/{id?}"` → `/User/10`

## Example
- `/About` → default route
- `/info` → custom route
- `/User/10` → route parameter (UserId = 10)
- `/User` → optional parameter (UserId is null)
