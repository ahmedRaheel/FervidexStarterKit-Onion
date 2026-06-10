# Onion Architecture Layout

This solution is separated into five projects:

```text
src/
├── StarterKit.Api                      # Presentation layer: Minimal APIs, Swagger, middleware, observability wiring
├── StarterKit.UseCase                  # Application/use cases: CQRS commands, queries, handlers, validators, interfaces
├── StarterKit.Domain                   # Domain model: entities, base classes, value objects, domain contracts
├── StarterKit.Infrastructure.Data      # Database implementation: EF Core DbContext, configurations, seed, connection factory
└── StarterKit.Infrastructure.External  # External services: Redis, HTTP resilience, Serilog configuration
```

Dependency direction:

```text
API → UseCase → Domain
API → Infrastructure.Data → UseCase + Domain
API → Infrastructure.External → UseCase
```

Rules:

- Domain does not depend on any other project.
- UseCase depends only on Domain and framework abstractions/packages needed for handlers and validation.
- Infrastructure.Data implements UseCase abstractions for persistence.
- Infrastructure.External implements UseCase abstractions for external integrations.
- API composes everything through dependency injection and maps endpoints only.
```
