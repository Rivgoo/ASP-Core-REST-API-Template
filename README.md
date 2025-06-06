# => ASP.NET Core 9 Web API Template <=

A starter template for building RESTful APIs using ASP.NET Core 9. This project implements a Clean Architecture approach and includes common features like EF Core for data access (with MySQL), JWT authentication, ASP.NET Core Identity, API versioning, and Swagger for API documentation.

## ✨ Project Overview

*   **Framework:** ASP.NET Core 9
*   **Architecture:** Clean Architecture (Domain, Application, Infrastructure, Web.API)
*   **Database:** Entity Framework Core with MySQL 🐘
*   **Authentication:** JWT Bearer Tokens 🛡️ & ASP.NET Core Identity
*   **API Documentation:** Swagger/OpenAPI 📜 with versioning support
*   **Key Features:**
    *   👤 User registration and login
    *   🔑 Role-based authorization
    *   ⚙️ Basic CRUD operations for users (example)
    *   📊 Filtering and pagination for user queries
    *   ⏱️ Automatic audit trail for entities (CreatedAt, UpdatedAt)

## 🏁 Getting Started

### Prerequisites

*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [MySQL Server](https://dev.mysql.com/downloads/mysql/) (or another compatible database if you adapt the EF Core provider)

### 🛠️ Configuration

1.  **Connection String:**
    Update the `DataBaseConnection` string in `src/Web.API/appsettings.Development.json` (and `appsettings.json` for production) to point to your MySQL instance.
    ```json
    "ConnectionStrings": {
      "DataBaseConnection": "Server=localhost;Port=3306;Database=YOUR_DB_NAME;Uid=YOUR_USER;Pwd=YOUR_PASSWORD;"
    }
    ```
2.  **Initial Admin (Optional):**
    The `InitialAdmin` section in `appsettings.Development.json` can be configured to create an administrator user on first run.

### 🗄️ Database Setup

1.  **Apply Migrations:**
    Navigate to the `src/Infrastructure` directory in your terminal:
    ```bash
    cd src/Infrastructure
    ```
    Ensure the `Web.API` project is set as the startup project if running `dotnet ef` commands that require it, or specify it:
    ```bash
    dotnet ef database update --startup-project ../Web.API
    ```
    This will create the database (if it doesn't exist) and apply all pending migrations. By default, `ApplyMigrations` is set to `true` in `appsettings.json` which attempts this on startup, but manual application is more controlled.

### 🏃 Running the Application

1.  **Navigate to the Web.API project:**
    ```bash
    cd src/Web.API
    ```
2.  **Run the application:**
    ```bash
    dotnet run
    ```
    By default, the application will run on `http://localhost:5000` and `https://localhost:5001` (if HTTPS is configured for development).

3.  **Access Swagger UI:**
    Open your browser and navigate to `https://localhost:5001/swagger` (or the configured HTTPS URL + `/swagger`) to view the API documentation and test endpoints.