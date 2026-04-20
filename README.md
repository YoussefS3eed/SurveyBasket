<p align="center">
  <h1 align="center">📊 SurveyBasket</h1>
  <p align="center">
    A robust, production-ready REST API for creating and managing surveys, polls, and collecting user votes — built with Clean Architecture principles on .NET 10.
  </p>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10" />
  <img src="https://img.shields.io/badge/C%23-13-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C# 13" />
  <img src="https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
  <img src="https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge" alt="License" />
</p>

---

## 📑 Table of Contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [API Endpoints](#-api-endpoints)
- [Authentication & Authorization](#-authentication--authorization)
- [Background Jobs](#-background-jobs)
- [Health Checks](#-health-checks)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🔍 Overview

**SurveyBasket** is a full-featured Web API that allows administrators to create polls with questions and answers, publish them to members, collect votes, and analyze results. The system supports role-based access control with a fine-grained permission system, email verification, rate limiting, API versioning, and much more.

---

## 🏗️ Architecture

The project follows **Clean / Onion Architecture** with clear separation of concerns across four layers:

```
┌──────────────────────────────────────────────┐
│                  API Layer                   │
│         (Controllers, Middleware,            │
│          Filters, Program.cs)                │
├──────────────────────────────────────────────┤
│             Infrastructure Layer             │
│     (EF Core, Identity, JWT, Hangfire,       │
│      Email, Caching, Repositories)           │
├──────────────────────────────────────────────┤
│             Application Layer                │
│      (CQRS Commands/Queries, MediatR,        │
│       Validation, Mapping, Behaviors)        │
├──────────────────────────────────────────────┤
│               Domain Layer                   │
│       (Entities, Interfaces, Errors,         │
│          Common, Base Entities)              │
└──────────────────────────────────────────────┘
```

**Dependency Flow:** `API → Infrastructure → Application → Domain`

---

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| **Framework** | .NET 10 / ASP.NET Core 10 |
| **Language** | C# 13 |
| **ORM** | Entity Framework Core 10 |
| **Database** | SQL Server (LocalDB) |
| **Authentication** | ASP.NET Core Identity + JWT Bearer Tokens |
| **Authorization** | Custom Permission-Based Policy System |
| **Mediator / CQRS** | MediatR 14 |
| **Validation** | FluentValidation 12 |
| **Object Mapping** | Mapster 7.4 |
| **Background Jobs** | Hangfire 1.8 |
| **Email** | MailKit 4.15 |
| **Caching** | HybridCache (in-memory) |
| **Logging** | Serilog |
| **API Documentation** | Swagger / Swashbuckle |
| **API Versioning** | Asp.Versioning |
| **Health Checks** | AspNetCore.HealthChecks (SQL Server, Hangfire, Mail) |

---

## ✨ Features

### Core Functionality
- 📋 **Poll Management** — Create, update, delete, publish/unpublish polls with start and end dates
- ❓ **Question Management** — Add and manage questions within polls
- ✅ **Answer Management** — Define multiple-choice answers for questions
- 🗳️ **Voting System** — Members can vote on published polls with answer selection
- 📊 **Results & Analytics** — View raw votes, votes per day, and votes per question

### Security & Access Control
- 🔐 **JWT Authentication** — Secure token-based auth with refresh token rotation
- 🛡️ **Permission-Based Authorization** — Fine-grained permission system with custom attributes
- 👥 **Role Management** — Admin, Member, and custom roles with assignable permissions
- 👤 **User Management** — Full user CRUD with profile management
- ✉️ **Email Verification** — Account confirmation via email with verification codes
- 🔑 **Password Management** — Forget/reset password flows

### Infrastructure
- ⚡ **Rate Limiting** — IP-based and user-based limiters with concurrency control
- 📌 **API Versioning** — Support for multiple API versions (v1, v2)
- 📝 **Structured Logging** — Serilog with console and file sinks
- 🏥 **Health Checks** — Database, Hangfire, and mail provider monitoring
- ⏰ **Background Jobs** — Recurring and fire-and-forget jobs with Hangfire dashboard
- 💾 **Caching** — HybridCache with cache invalidation via MediatR behaviors
- 🔄 **Unit of Work** — Transaction management pattern
- 🧩 **MediatR Pipeline Behaviors** — Validation, caching, and cache invalidation

---

## 📁 Project Structure

```
SurveyBasket/
├── 📄 SurveyBasket.slnx                    # Solution file
├── 📂 assets/                               # Static assets
├── 📂 src/
│   ├── 📂 SurveyBasket.Domain/              # Core domain layer
│   │   ├── Common/
│   │   │   ├── BaseEntities/                # AuditableEntity base class
│   │   │   ├── Dtos/                        # Domain-level DTOs
│   │   │   ├── Exceptions/                  # Custom exceptions
│   │   │   └── Models/                      # Domain models
│   │   ├── Entities/
│   │   │   ├── ApplicationUser.cs           # User entity (Identity)
│   │   │   ├── ApplicationRole.cs           # Role entity (Identity)
│   │   │   ├── Poll.cs                      # Poll entity
│   │   │   ├── Question.cs                  # Question entity
│   │   │   ├── Answer.cs                    # Answer entity
│   │   │   ├── Vote.cs                      # Vote entity
│   │   │   ├── VoteAnswer.cs                # Vote-Answer junction
│   │   │   ├── RefreshToken.cs              # Refresh token entity
│   │   │   └── EmailVerificationCode.cs     # Email verification
│   │   ├── Errors/                          # Domain error definitions
│   │   │   ├── Error.cs                     # Base error type
│   │   │   ├── PollErrors.cs
│   │   │   ├── QuestionErrors.cs
│   │   │   ├── UserErrors.cs
│   │   │   ├── VoteErrors.cs
│   │   │   ├── RoleErrors.cs
│   │   │   └── NotificationErrors.cs
│   │   └── Interfaces/
│   │       ├── IUnitOfWork.cs
│   │       └── Repositories/                # Repository contracts
│   │
│   ├── 📂 SurveyBasket.Application/         # Application / use-case layer
│   │   ├── Common/
│   │   │   ├── Behaviors/                   # MediatR pipeline behaviors
│   │   │   │   ├── ValidationBehavior.cs
│   │   │   │   ├── CachingBehavior.cs
│   │   │   │   └── CacheInvalidationBehavior.cs
│   │   │   ├── Caching/                     # Cache abstractions
│   │   │   ├── Constants/
│   │   │   ├── Contracts/                   # Request/response contracts
│   │   │   ├── Extensions/
│   │   │   ├── Interfaces/                  # Service interfaces
│   │   │   ├── Mappings/                    # Mapster configurations
│   │   │   └── Models/
│   │   ├── Features/                        # CQRS feature modules
│   │   │   ├── Answers/
│   │   │   ├── Authentication/
│   │   │   ├── Polls/
│   │   │   ├── Questions/
│   │   │   ├── Results/
│   │   │   ├── Roles/
│   │   │   ├── Users/
│   │   │   └── Votes/
│   │   └── DependencyInjection.cs
│   │
│   ├── 📂 SurveyBasket.Infrastructure/      # Infrastructure / data access layer
│   │   ├── Authorization/                   # Custom permission handler
│   │   ├── Configurations/                  # Hangfire configuration
│   │   ├── Health/                          # Custom health checks
│   │   ├── Persistence/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/              # EF Core entity configs
│   │   │   ├── Migrations/
│   │   │   ├── Repositories/                # Repository implementations
│   │   │   ├── Seeders/                     # Database seeders
│   │   │   ├── SP/                          # Stored procedures
│   │   │   └── UnitOfWork.cs
│   │   ├── Services/
│   │   │   ├── Auth/                        # JWT service
│   │   │   ├── BackgroundJobs/              # Hangfire job services
│   │   │   ├── Cache/                       # HybridCache implementation
│   │   │   ├── Email/                       # MailKit email service
│   │   │   └── Notification/                # Notification service
│   │   └── DependencyInjection.cs
│   │
│   └── 📂 SurveyBasket.API/                 # Presentation / API layer
│       ├── Abstractions/                    # Constants, permissions
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── AccountController.cs
│       │   ├── PollsController.cs
│       │   ├── QuestionsController.cs
│       │   ├── VotesController.cs
│       │   ├── ResultsController.cs
│       │   ├── RolesController.cs
│       │   └── UsersController.cs
│       ├── Extensions/
│       ├── Filters/                         # HasPermission attribute
│       ├── Middleware/
│       │   ├── ExceptionHandlingMiddleware.cs
│       │   └── SecurityStampValidationMiddleware.cs
│       ├── Program.cs
│       └── appsettings.json
│
└── 📂 SurveyBasket.Logs/                    # Log output directory
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (or any SQL Server instance)
- An SMTP service for email functionality (e.g., [Ethereal Email](https://ethereal.email/) for testing)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/YoussefS3eed/SurveyBasket.git
   cd SurveyBasket
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Configure User Secrets** (see [Configuration](#-configuration))
   ```bash
   cd src/SurveyBasket.API
   dotnet user-secrets set "Jwt:Key" "your-super-secret-key-at-least-32-characters"
   dotnet user-secrets set "EmailSettings:Password" "your-email-password"
   dotnet user-secrets set "HangfireSettings:Username" "admin"
   dotnet user-secrets set "HangfireSettings:Password" "admin-password"
   ```

4. **Apply migrations**
   ```bash
   dotnet ef database update --project ../SurveyBasket.Infrastructure --startup-project .
   ```

5. **Run the application**
   ```bash
   dotnet run --project src/SurveyBasket.API
   ```

6. **Open Swagger UI**
   ```
   https://localhost:{port}/swagger
   ```

---

## ⚙️ Configuration

The application uses `appsettings.json` with User Secrets for sensitive data. Key configuration sections:

| Section | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `ConnectionStrings:HangfireConnection` | Hangfire job storage connection |
| `Jwt` | JWT signing key, issuer, audience, and token expiry |
| `EmailSettings` | SMTP mail, display name, password, host, and port |
| `HangfireSettings` | Dashboard basic auth credentials |
| `Serilog` | Logging configuration (levels, sinks, enrichment) |
| `AllowedOrigins` | CORS allowed origins |

> ⚠️ **Important:** Never commit secrets like `Jwt:Key`, `EmailSettings:Password`, or `HangfireSettings` credentials. Use **User Secrets** for local development or environment variables for production.

---

## 📡 API Endpoints

### Authentication (`/Auth`)
| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/Auth` | Login with credentials |
| `POST` | `/Auth/register` | Register a new user |
| `POST` | `/Auth/confirm-email` | Confirm email address |
| `POST` | `/Auth/resend-confirmation` | Resend confirmation email |
| `PUT` | `/Auth/refresh-token` | Refresh JWT access token |
| `POST` | `/Auth/revoke-refresh-token` | Revoke a refresh token |
| `POST` | `/Auth/forget-password` | Request password reset |
| `POST` | `/Auth/reset-password` | Reset password |

### Polls (`/api/polls`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/polls` | Get all polls *(requires `GetPolls` permission)* |
| `GET` | `/api/polls/current` | Get current active polls *(v1 & v2)* |
| `GET` | `/api/polls/{id}` | Get poll by ID |
| `POST` | `/api/polls` | Create a new poll |
| `PUT` | `/api/polls/{id}` | Update a poll |
| `DELETE` | `/api/polls/{id}` | Delete a poll |
| `PUT` | `/api/polls/{id}/togglePublish` | Toggle poll publish status |

### Questions (`/api/polls/{pollId}/questions`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/polls/{pollId}/questions` | Get all questions for a poll |
| `GET` | `/api/polls/{pollId}/questions/{id}` | Get question by ID |
| `POST` | `/api/polls/{pollId}/questions` | Add a question |
| `PUT` | `/api/polls/{pollId}/questions/{id}` | Update a question |

### Voting (`/api/polls/{pollId}/vote`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/polls/{pollId}/vote` | Get available questions to vote on |
| `POST` | `/api/polls/{pollId}/vote` | Submit a vote |

### Results (`/api/polls/{pollId}/results`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/polls/{pollId}/results/raw-data` | Get raw vote data |
| `GET` | `/api/polls/{pollId}/results/votes-per-day` | Get votes per day breakdown |
| `GET` | `/api/polls/{pollId}/results/votes-per-question` | Get votes per question stats |

### User Management (`/api/users`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/users` | List all users |
| `GET` | `/api/users/{id}` | Get user by ID |
| `POST` | `/api/users` | Create a user |
| `PUT` | `/api/users/{id}` | Update user details |

### Account (`/api/account`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/account` | Get current user profile |
| `PUT` | `/api/account` | Update current user profile |
| `PUT` | `/api/account/password` | Change password |

### Role Management (`/api/roles`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/roles` | Get all roles |
| `POST` | `/api/roles` | Create a role |
| `PUT` | `/api/roles/{id}` | Update a role |

---

## 🔐 Authentication & Authorization

### Authentication Flow
1. **Register** → User receives a confirmation email
2. **Confirm Email** → Account becomes active
3. **Login** → Receive JWT access token + refresh token
4. **Refresh** → Exchange refresh token for a new access token
5. **Revoke** → Invalidate refresh token on logout

### Authorization Model
The system uses a **custom permission-based authorization** system:

- **Roles** — Predefined (`Admin`, `Member`) and custom roles
- **Permissions** — Granular permissions (e.g., `GetPolls`, `AddPolls`, `UpdatePolls`, `DeletePolls`, `Results`)
- **`[HasPermission]`** — Custom attribute for controller action authorization
- **Security Stamp Validation** — Middleware validates security stamps to enforce real-time access revocation

---

## ⏰ Background Jobs

The application uses **Hangfire** for background job processing:

- **Recurring jobs** — Automatically scheduled on application startup
- **Dashboard** — Available at `/jobs` (protected with basic authentication)
- **Persistent storage** — Jobs are stored in a dedicated SQL Server database

---

## 🏥 Health Checks

Health monitoring is available at the `/health` endpoint with detailed status for:

| Check | Description |
|---|---|
| **Database** | SQL Server connectivity check |
| **Hangfire** | Minimum available server validation |
| **Mail Service** | SMTP provider connectivity check |

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

<p align="center">
  Made with ❤️ by <a href="https://github.com/YoussefS3eed">Youssef S3eed</a>
</p>
