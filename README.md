# 🌐 MitoCode Microservices with .NET 9

This repository is part of the final project for the microservices training at [MitoCode](https://mitocode.com/), focused on deeply exploring microservice architecture—its benefits, challenges, and how to tackle it comprehensively using .NET 9. Throughout the course, we worked with various technologies related to persistence, inter-service communication, and security, allowing hands-on experience in a distributed and decoupled environment.

One of the most enriching aspects was observing how different architectural approaches can coexist within the same ecosystem—leveraging multiple API paradigms and combining both relational and NoSQL data storage technologies.

This experience not only provided new knowledge but also reinforced prior skills, requiring adaptation to a completely different paradigm from monolithic development and embracing the inherent complexity of distributed systems.

---

## ⚙️ Core Technologies

| Technology              | Description                                                  |
|-------------------------|--------------------------------------------------------------|
| ![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet?logo=dotnet)                 | Development platform for building modern APIs. |
| ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoft-sql-server&logoColor=white) | Relational database for structured persistence. |
| ![MongoDB](https://img.shields.io/badge/MongoDB-47A248?logo=mongodb&logoColor=white)  | NoSQL database for flexibility and scalability. |
| ![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?logo=rabbitmq&logoColor=white) | Messaging system for asynchronous communication. |
![MassTransit](https://img.shields.io/badge/MassTransit-5D0C91?logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgdmlld0JveD0iMCAwIDEwMCAxMDAiPjxjaXJjbGUgY3g9IjUwIiBjeT0iNTAiIHI9IjQwIiBmaWxsPSIjZTZkMmY2Ii8+PHRleHQgeD0iNTAiIHk9IjU2IiBmb250LXNpemU9IjI0IiBmb250LWZhbWlseT0iQXJpYWwiIGZpbGw9IndoaXRlIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIj5NVDwvdGV4dD48L3N2Zz4=) | Integration of asynchronous flows and SAGA coordination. |
| ![JWT](https://img.shields.io/badge/JWT-000000?logo=json-web-tokens&logoColor=white)  | Token-based security for authentication and authorization. |
| ![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)     | Service containerization. |
| 🧩 **Razor Pages**       | Simple web interface within the .NET ecosystem, used for the login screen. |

---

## 📚 Tools & Frameworks

- 🧵 **Entity Framework Core** – ORM for data access using Code First and DB First approaches.
- 🧠 **Duende IdentityServer** – Identity management with OAuth2/OpenID.
- 📬 **MassTransit** – Messaging integration with RabbitMQ, supports SAGA orchestration.
- 📖 **Swagger** – Interactive API documentation.
- 🙋‍♂️ **ASP.NET Core Identity** – User, role, and claims management.
- 🧪 **Postman** – API endpoint testing and validation.

---

## 🧠 Applied Design Patterns

This project applies several design patterns that improve maintainability, scalability, and clarity of the system:

- 📢 **Publish/Subscribe** – Decoupled communication between services via events.
- ⚙️ **Factory** – Flexible object creation based on context or configuration.
- 🧩 **Options Pattern** – Strongly typed service configuration.
- 🔄 **State Pattern** – State handling for long-running processes (SAGA).
- 🗃️ **Repository Pattern** – Abstraction for data access logic.
- 🧵 **SAGA Pattern** – Distributed transaction coordination across microservices.

---

## 📌 Features & Activities

This project not only implements a functional architecture but also reflects the learning path followed during the course:

- 🧱 **Entity Framework Core (Code First & DB First)** – Both approaches were used depending on the microservice needs.
- 📡 **Asynchronous Messaging with RabbitMQ** – Domain events and decoupled flows.
- 🧵 **Orchestration with MassTransit** – Integration of asynchronous flows and coordination using SAGA.
- 📦 **REST APIs & Minimal APIs** – Lightweight and expressive endpoint development.
- 🧱 **Layered Architecture** – Clear separation between presentation, business logic, and data layers.
- 🧰 **Custom Libraries (NuGet)** – Code reuse via custom-built packages.
- 🎯 **Authentication & Authorization** – Secure implementation using JWT, IdentityServer, and Identity.
- 🧩 **Domain Events** – Reactions to business-related state changes and actions.
- 🧵 **Distributed Transactions** – Coordinated using SAGA and persisted based on the database type.

---

## 📁 General Project Structure

```plaintext
├── services/                # Independent microservices
│   ├── ProductService/      # Product microservice
│   ├── StockService/        # Stock microservice
│   ├── OrderService/        # Orders microservice
│   ├── PaymentService/      # Payments microservice
│   └── AuthService/         # Authentication and authorization microservice
├── common/                  # Shared libraries and NuGet packages
│   └── Shared libraries/    # Shared.Library and Shared.Events authored libraries
├── infrastructure/          # Implementations for databases, messaging, etc.
│   └── docker-compose.yml   # Service orchestration
├── docs/                    # Documentation, diagrams, and scripts
│   ├── SqlScripts/          # SQL Server scripts
│   └── Nuget-Package/       # Custom .nupkg libraries used in the project
└── README.md                # This file
```
---

## 📜 License
- This project is for educational purposes only and does not have a specific license.

---
---

## 🌍 Versión en Español

---

# 🌐 MitoCode Microservicios con .NET 9

Este repositorio forma parte del proyecto final de la capacitacion de microservicios de [MitoCode](https://mitocode.com/) enfocada en explorar en profundidad la arquitectura de microservicios: sus ventajas, desafíos y cómo abordarla de forma integral desde .NET 9. A lo largo del curso, se trabajó con diversas tecnologías orientadas a la persistencia, la comunicación entre servicios y la seguridad, permitiendo experimentar en un entorno distribuido y desacoplado.
Uno de los aspectos más enriquecedores fue ver cómo diferentes arquitecturas pueden convivir dentro del mismo ecosistema, utilizando múltiples enfoques para las APIs y combinando tecnologías de almacenamiento relacional y NoSQL.
Esta experiencia no solo me brindó nuevos conocimientos, sino que también reforzó habilidades previas, obligándome a adaptarme a un paradigma completamente distinto al desarrollo monolítico y a enfrentar la complejidad propia de los sistemas distribuidos.
>

---

## ⚙️ Tecnologías Principales

| Tecnología             | Descripción                                                  |
|------------------------|--------------------------------------------------------------|
| ![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet?logo=dotnet)                 | Plataforma de desarrollo para construir APIs modernas. |
| ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoft-sql-server&logoColor=white) | Base de datos relacional para persistencia estructurada. |
| ![MongoDB](https://img.shields.io/badge/MongoDB-47A248?logo=mongodb&logoColor=white)  | Base de datos NoSQL para flexibilidad y escalabilidad. |
| ![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?logo=rabbitmq&logoColor=white) | Sistema de mensajería para comunicación asincrónica. |
![MassTransit](https://img.shields.io/badge/MassTransit-5D0C91?logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgdmlld0JveD0iMCAwIDEwMCAxMDAiPjxjaXJjbGUgY3g9IjUwIiBjeT0iNTAiIHI9IjQwIiBmaWxsPSIjZTZkMmY2Ii8+PHRleHQgeD0iNTAiIHk9IjU2IiBmb250LXNpemU9IjI0IiBmb250LWZhbWlseT0iQXJpYWwiIGZpbGw9IndoaXRlIiB0ZXh0LWFuY2hvcj0ibWlkZGxlIj5NVDwvdGV4dD48L3N2Zz4=) | Integración de flujos asíncronos y coordinación de SAGA. |
| ![JWT](https://img.shields.io/badge/JWT-000000?logo=json-web-tokens&logoColor=white)  | Seguridad basada en tokens para autenticación y autorización. |
| ![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)     | Contenerización de servicios. |
| 🧩 **Razor Pages**       | Interfaz web simple dentro del ecosistema .NET utilizada para la pantalla de login. |

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
├── docs/                    # Documentacion, diagramas y scripts
│   ├── SqlScripts/          # Scripts de SqlServer
│   └── Nuget-Package/       # Librerias de autoria .nupkg utilizadas en el proyecto
└── README.md                # Este archivo
```
---

## 📜 Licencia
Este proyecto es solo para fines educativos y no tiene una licencia específica.