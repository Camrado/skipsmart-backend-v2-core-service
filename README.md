<div align="center">

# 🎓 SkipSmart Core Service

**The robust backend engine powering the SkipSmart platform.**<br>
Helping UFAZ University students seamlessly manage and track their absence limits.

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-2b2d38?style=for-the-badge&logo=gitkraken&logoColor=white)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

</div>

## 📖 Overview

**SkipSmart Core Service** is the central backend API for the SkipSmart application. It handles business logic, data persistence, and external service integrations to provide a reliable, scalable, and secure platform for student attendance management.

## ✨ Key Features

- **Robust Business Logic**: Manages complex domain rules for student attendance, groups, and courses.
- **Secure Authentication**: Protected endpoints with robust JWT Bearer authentication and Role-Based Access Control.
- **RESTful API**: Well-structured Web API with Swagger/OpenAPI interactive documentation and API versioning.
- **Automated Scheduling**: Background jobs and scheduled tasks powered by Quartz.NET.
- **Email Notifications**: Integrated email services for user verification and platform alerts.

## 🏗 Architecture

This project strictly adheres to **Clean Architecture** principles, ensuring separation of concerns, testability, and independence from frameworks and databases. The solution is divided into the following layers:

- **Domain**: Contains enterprise logic, core entities, value objects, and domain exceptions.
- **Application**: Contains business logic, CQRS commands/queries (MediatR), validation, and interfaces.
- **Infrastructure**: Implements data access (EF Core), database migrations, external API clients, and background jobs.
- **Api**: The entry point, handling HTTP requests, routing, and dependency injection.

## 🛠 Tech Stack

### Core Technologies
- **Framework:** [.NET 10 Web API](https://dotnet.microsoft.com/)
- **Architecture:** Clean Architecture + CQRS Pattern
- **Database:** [PostgreSQL](https://www.postgresql.org/)

### Libraries & Tools
- **ORM:** Entity Framework Core
- **CQRS & Mediator:** MediatR
- **Validation:** FluentValidation
- **Authentication:** ASP.NET Core JWT Bearer
- **API Documentation:** Swashbuckle (Swagger), ASP.NET Core API Explorer
- **Scheduling:** Quartz.NET
- **Email Services:** MailKit, Mailjet
- **Testing:** xUnit, Coverlet

## 📁 Project Structure

```text
src/
├── SkipSmart.Api/            # Presentation layer (Controllers, Middleware)
├── SkipSmart.Application/    # Use cases, MediatR Handlers, Validation
├── SkipSmart.Domain/         # Core Entities, Value Objects, Domain Events
├── SkipSmart.Infrastructure/ # EF Core DbContext, Migrations, Services
Test/
└── SkipSmart.Domain.Tests/   # Unit tests (and other test projects)
```

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- IDE (Visual Studio, Rider, or VS Code)

### Running Locally

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd skipsmart-backend-v2-core-service
   ```

2. **Configure Environment:**
   Ensure you have configured your database connection strings and JWT secrets in `appsettings.json` or via environment variables / `.env` files.

3. **Run Migrations:**
   Navigate to the solution root and apply database migrations:
   ```bash
   dotnet ef database update --project src/SkipSmart.Infrastructure --startup-project src/SkipSmart.Api
   ```

4. **Start the API:**
   ```bash
   dotnet run --project src/SkipSmart.Api
   ```

5. **Explore the API:**
   Navigate to `https://localhost:<port>/swagger` in your browser to view the interactive API documentation and test the endpoints.

## 🔗 Related Repositories

- 🗓️ **[Edupage Timetable Service](https://github.com/Camrado/skipsmart-backend-v2-timetable-service)**: A separate microservice built with Python (Flask) dedicated to retrieving timetable data from Edupage.
