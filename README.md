# SkipSmart Core Service

SkipSmart Core Service is the main backend service of the SkipSmart project, dedicated to helping UFAZ University students manage their absence limits efficiently. Built with .NET 8, this service handles the core functionality of the SkipSmart system, using PostgreSQL for reliable and scalable data storage.

## Features

- **Core Functionality**
    - Manages the main backend services of the SkipSmart project.
- **Database**
    - Uses PostgreSQL to ensure reliable and scalable data storage.
- **Clean Architecture**
    - Implements Clean Architecture principles for a maintainable and scalable codebase.
- **Web API with Controllers**
    - Provides RESTful endpoints using .NET 8 Web API with Controllers.

## Tech Stack

- **Framework**: .NET 8
- **Architecture**: Clean Architecture
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Micro ORM**: Dapper
- **Logging**: Serilog
- **Unit Testing**: xUnit

## Related Repositories

- **Edupage Timetable Service**
    - The functionality for retrieving the timetable from Edupage is handled in a separate [service written on Python using Flask](https://github.com/Camrado/skipsmart-backend-v2-timetable-service).
