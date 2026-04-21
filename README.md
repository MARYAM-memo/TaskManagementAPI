# 📝 Simple Task Management API

A clean, secure RESTful API for managing personal tasks, built with **ASP.NET Core Web API** and **Clean Architecture**.

## 🚀 Features

- **JWT Authentication** & Role-based Authorization
- CRUD operations on Tasks (Create, Read, Update, Delete)
- Each user can only access their own tasks
- **Fluent Validation** for input validation
- **AutoMapper** for object mapping
- **Swagger/OpenAPI** documentation
- **Postman Collection** included

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core 10, C#
- **Database:** SQL Server / PostgreSQL (choose one)
- **ORM:** Entity Framework Core (Code First)
- **Design Patterns:** Repository, Unit of Work, Dependency Injection
- **Tools:** Swagger, Postman, Git

## 🏗️ Project Structure
TaskManagementAPI/
└── TodoAPI/ # Controllers, DTOs, Data[Mapping/Migrations/Seed], DataAccess, Extensions, Interfaces, Models, Services, Settings, Validators, Program.cs


## 🧪 API Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST   | /api/auth/register | Register new user | No |
| POST   | /api/auth/login    | Login and get JWT token | No |
| GET    | /api/Task         | Get all tasks for logged user | Yes |
| GET    | /api/Task/{id}    | Get task by ID | Yes |
| POST   | /api/Task/create         | Create new task | Yes |
| PUT    | /api/Task/edit/{id}    | Update task | Yes |
| DELETE | /api/Task/delete/{id}    | Delete task | Yes |

## 📦 Getting Started
### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/MARYAM-memo/TaskManagementAPI.git
   cd TaskManagementAPI
2. Update the database connection string in API/appsettings.json
3. Apply migrations
   ```bash
   dotnet ef database update --project Infrastructure --startup-project API
4. Run the API
   ```bash
   cd API
   dotnet run
5. Open Swagger UI: Navigate to https://localhost:7001/swagger


## Contact
Marim Mohamed - GitHub - marimeltaweel26@gmail.com

Project Link: https://github.com/MARYAM-memo/TaskManagementAPI.git


