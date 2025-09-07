# Razor Pages – Dependency Injection & Services

## Key Concepts
- **Dependency Injection (DI)** allows injecting services into PageModels.
- Register services in **Program.cs**: `AddSingleton`, `AddScoped`, `AddTransient`.
- Inject via **constructor injection**.
- Use cases: logging, data access, utilities, repository pattern.

## Example
- `IMessageService` → interface
- `MessageService` → implementation
- Inject into `IndexModel` → display welcome message
