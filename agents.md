# Guía para agentes

## Contexto

Este repositorio es la solución `NetDemo`, organizada en proyectos bajo `src/`. Antes de modificar archivos, revisa esta guía y la documentación arquitectónica en `docs/architecture-decisions.md`.

## Tecnologías

- .NET 10 y ASP.NET Core MVC.
- Razor Views (`.cshtml`), Bootstrap 5, HTML5 y CSS3.
- JavaScript nativo, sin jQuery. Usa Fetch API cuando una funcionalidad necesite comunicación asíncrona.
- Incorpora SweetAlert2 solo si una funcionalidad concreta justifica esa dependencia.

## Arquitectura

- `NetDemo.Domain`: entidades, reglas de negocio y contratos de repositorio. No debe depender de otros proyectos.
- `NetDemo.Application`: casos de uso y contratos de aplicación. Puede depender de Domain.
- `NetDemo.Infrastructure`: implementaciones de repositorios e integraciones externas. Puede depender de Application y Domain.
- `NetDemo.WebApp`: controladores, ViewModels de presentación, vistas Razor y activos del navegador. Los controladores delegan la lógica a casos de uso.
- WebApp es el punto de composición y puede referenciar Infrastructure para configurar implementaciones. No uses Infrastructure desde controladores, vistas ni lógica de presentación.
- Mantén las reglas de negocio en Domain y la coordinación de casos de uso en Application.

## Frontend

- Usa `Views/Shared/_Layout.cshtml` para el layout general y partials compartidos para navegación y elementos reutilizables.
- Agrupa vistas y partials por funcionalidad; conserva los ViewModels en WebApp.
- Mantén estilos globales en `wwwroot/css/site.css` y estilos específicos en `wwwroot/css/features/`.
- Mantén JavaScript compartido en `wwwroot/js/` y módulos específicos en `wwwroot/js/features/`.
- No agregues jQuery ni mezcles Bootstrap con otros frameworks CSS.

## Cambios y dependencias

- No inventes requisitos funcionales ni agregues paquetes o servicios externos sin una necesidad concreta.
- Documenta las decisiones arquitectónicas relevantes en `docs/architecture-decisions.md`.
- Sigue las instrucciones del usuario sobre autorización y verificación. No crees ni ejecutes pruebas salvo que el usuario pida probar o verificar; una solicitud explícita de compilación sí autoriza compilar.
