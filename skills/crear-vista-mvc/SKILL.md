---
name: crear-vista-mvc
description: Plan and implement an ASP.NET Core MVC feature with Razor views, presentation models, and browser assets. Use when creating or changing a screen in an existing MVC application.
---

# Crear vista MVC

Plan and implement the requested MVC screen using the conventions and architecture of the existing solution. Do not infer business fields, operations, validation rules, or data sources that the user has not specified.

## Before implementation

1. Read repository guidance such as `agents.md` and architecture documentation. Inspect the solution projects, their references, the current MVC startup, and nearby screens or modules that establish local conventions.
2. Identify which parts of the request are known and which functional requirements are missing. Ask concise questions about missing fields, operations, validation, or interaction flow when they affect the design. Do not create placeholder business fields or sample operations to fill gaps.
3. Propose the screen structure, form behavior, files to create or change, layer responsibilities, and any JavaScript or dependency needs. Wait for the user's approval before modifying files.

## Architecture and implementation

- Keep Razor views, controllers, presentation ViewModels, CSS, and browser JavaScript in the WebApp.
- Keep controllers thin: they bind requests, invoke Application contracts/use cases, and choose the response. Do not put business rules or persistence access in controllers or views.
- Keep business rules and repository contracts in Domain; coordinate confirmed operations in Application; implement persistence or external adapters in Infrastructure. Preserve the repository's dependency direction and use the WebApp only as the composition root for Infrastructure.
- Use the existing shared layout and Bootstrap components. Keep feature-specific Razor partials, CSS, and JavaScript grouped with the feature according to repository conventions.
- Use native JavaScript without jQuery. Add Fetch API only for confirmed asynchronous interactions. Add SweetAlert2 or other dependencies only when the confirmed UX justifies them.
- Do not implement unrequested persistence, authentication, API endpoints, business operations, or extra dependencies.

## Completion

After approval, implement the agreed scope and summarize the files changed and architectural choices. Run a build or other verification only when the user requests verification; do not add or run tests unless requested.
