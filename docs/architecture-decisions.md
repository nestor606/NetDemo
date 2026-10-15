# Decisiones de arquitectura

## ADR-001: capas y dirección de dependencias

- `NetDemo.Domain` es independiente y contiene el modelo, reglas de negocio y contratos de repositorio.
- `NetDemo.Application` depende de Domain y coordina casos de uso.
- `NetDemo.Infrastructure` depende de Application y Domain para implementar adaptadores.
- `NetDemo.WebApp` depende de Application e Infrastructure. La referencia a Infrastructure se limita a la composición y configuración en el punto de entrada; controladores y vistas dependen de contratos de Application.
- La lógica de negocio permanece en Domain y los controladores delegan el trabajo a casos de uso.

No se agregan implementaciones de persistencia, integraciones ni casos de uso hasta que exista un requisito funcional que los necesite.

## ADR-002: presentación MVC

`WebApp` usa controladores MVC y Razor. El layout, navegación y partials compartidos viven bajo `Views/Shared`; cada funcionalidad puede agrupar sus vistas y partials. Los ViewModels son exclusivos de presentación.

## ADR-003: activos del navegador

Bootstrap 5 se sirve desde jsDelivr. El código propio reside en `wwwroot/css` y `wwwroot/js`, con carpetas para estilos y módulos por funcionalidad. Se usa JavaScript nativo; no se incluye jQuery. Los formularios pueden usar validación del lado servidor y atributos HTML/Razor sin incorporar validadores que dependan de jQuery. Fetch API, SweetAlert2 y módulos de funcionalidad se incorporarán cuando una funcionalidad los justifique.

## ADR-004: dependencias externas

Los proyectos no incorporan paquetes NuGet de terceros. Bootstrap es la única dependencia del frontend y se referencia como recurso estático externo; esto requiere conexión del cliente a jsDelivr. No se elige motor de base de datos ni servicio externo sin requisitos que lo sustenten.

## ADR-005: persistencia local de tipos de mercado

Los tipos de mercado se guardan en Infrastructure como un archivo JSON por registro, bajo `App_Data/TipoMercado` (configurable mediante `Storage:TipoMercadoDirectory`). El nombre del archivo usa el GUID del registro. El alta asigna el estado `Activo`; editar actualiza nombre y estado; eliminar borra el archivo del registro. Los archivos permanecen fuera de `wwwroot` y el acceso se realiza mediante el contrato de repositorio definido en Domain.
