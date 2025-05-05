# 🌐 MitoCode Microservicios con .NET 9

Este repositorio forma parte del proyecto final de la capacitacion de microservicios de [MitoCode](https://mitocode.com/) enfocada en explorar en profundidad la arquitectura de microservicios: sus ventajas, desafíos y cómo abordarla de forma integral desde .NET 9. A lo largo del curso, se trabajó con diversas tecnologías orientadas a la persistencia, la comunicación entre servicios y la seguridad, permitiendo experimentar en un entorno distribuido y desacoplado.
Uno de los aspectos más enriquecedores fue ver cómo diferentes arquitecturas pueden convivir dentro del mismo ecosistema, utilizando múltiples enfoques para las APIs y combinando tecnologías de almacenamiento relacional y NoSQL.
Esta experiencia no solo me brindó nuevos conocimientos, sino que también reforzó habilidades previas, obligándome a adaptarme a un paradigma completamente distinto al desarrollo monolítico y a enfrentar la complejidad propia de los sistemas distribuidos.
>

---

## ⚙️ Tecnologías Principales

| Tecnología            | Descripción                                      |
|----------------------|--------------------------------------------------|
| 🟦 **.NET 9**         | Plataforma de desarrollo para construir APIs modernas. |
| 🐘 **SQL Server**     | Base de datos relacional para persistencia estructurada. |
| 🍃 **MongoDB**        | Base de datos NoSQL para flexibilidad y escalabilidad. |
| 🐇 **RabbitMQ**       | Sistema de mensajería para comunicación asincrónica. |
| 🔐 **OAuth 2.0 / JWT**| Seguridad basada en tokens para autenticación y autorización. |
| 🐳 **Docker / Compose**| Contenerización y orquestación de servicios. |
| 🧩 **Razor Pages**    | Interfaz web simple y rápida dentro del ecosistema .NET. |

---

## 📚 Herramientas & Frameworks

- 🧵 **Entity Framework Core** – ORM para manejar datos con enfoque Code First y DB First.
- 🧠 **Duende IdentityServer** – Gestión de identidad con OAuth2/OpenID.
- 📬 **MassTransit** – Integración de mensajería con RabbitMQ, soporte para SAGA.
- 📖 **Swagger** – Documentación interactiva para APIs.
- 🙋‍♂️ **ASP.NET Core Identity** – Gestión de usuarios, roles y claims.
- 🧪 **Postman** – Pruebas y validación de endpoints de API.

---

## 🧠 Patrones Aplicados

El proyecto aplica varios patrones de diseño que mejoran la mantenibilidad, escalabilidad y claridad del sistema:

- 📢 **Publish/Subscribe** – Comunicación desacoplada entre servicios con eventos.
- ⚙️ **Factory** – Creación flexible de objetos según contexto o configuración.
- 🧩 **Options Pattern** – Configuración de servicios fuertemente tipada.
- 🔄 **State Pattern** – Manejo de estados en procesos largos (SAGA).
- 🗃️ **Repository Pattern** – Abstracción de la lógica de acceso a datos.
- 🧵 **SAGA Pattern** – Coordinación de transacciones distribuidas entre microservicios.

---

## 📌 Funcionalidades y Actividades Realizadas

Este proyecto no solo implementa una arquitectura funcional, sino que también refleja el camino de aprendizaje durante el curso:

- 🧱 **Entity Framework Core (Code First & DB First)**: Ambos enfoques fueron utilizados según la necesidad del microservicio.
- 📡 **Mensajería Asíncrona con RabbitMQ**: Eventos de dominio y flujos desacoplados.
- 📦 **APIs REST & Minimal APIs**: Desarrollo de endpoints ligeros y expresivos.
- 🧱 **Arquitectura en Capas**: Separación clara entre presentación, lógica y datos.
- 🧰 **Librerías Propias (NuGet)**: Reutilización de código mediante paquetes personalizados.
- 🎯 **Autenticación y Autorización**: Implementación segura con JWT, IdentityServer y Identity.
- 🧩 **Eventos de Dominio**: Reacción a cambios y acciones dentro del negocio.
- 🧵 **Transacciones Distribuidas**: Coordinadas con SAGA y persistidas según el tipo de base de datos.

---

## 📁 Estructura General del Proyecto

```plaintext

├── services/                # Microservicios independientes
│   ├── ProductService/      # Servicio de Productos
│   ├── StockService/        # Servicio de Stock
│   ├── OrderService/        # Servicio de Ordenes
│   ├── PäymentService/      # Servicio de Pagos
│   └── AuthService/         # Servicio de autenticacion y autorizacion
├── common/                  # Librerías compartidas y paquetes NuGet
│   └── Shared libraries/    # Librerias de autoria Shared.Library y Shared.Events
├── infrastructure/          # Implementaciones de base de datos, mensajería, etc.
│   └── docker-compose.yml   # Orquestación de servicios
└── README.md                # Este archivo