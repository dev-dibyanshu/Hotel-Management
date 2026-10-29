# IdentityService

Identity and Authentication microservice for Hotel Management System.

## Overview

This service handles user registration, authentication, and JWT token generation for the Hotel Management System.

## Architecture

Simple layered architecture with the following structure:

```
Controller → Service → Repository → DbContext → SQL Server
```

## Features

- User registration with email uniqueness validation
- User login with password verification
- JWT token generation and validation
- Role-based authorization (Guest, Staff, Manager, Admin)
- Protected endpoint for retrieving current user information

## Tech Stack

- ASP.NET Core 8.0 Web API
- Entity Framework Core 8.0
- SQL Server
- JWT Bearer Authentication
- Swagger/OpenAPI

## Project Structure

```
IdentityService/
├── Controllers/          # API endpoints
├── DTOs/                # Data transfer objects
├── Data/                # DbContext
├── Migrations/          # EF Core migrations
├── Models/              # Domain models
├── Repositories/        # Data access layer
├── Services/            # Business logic
└── Program.cs           # Application entry point
```

## Configuration

Update `appsettings.json` or `appsettings.Development.json` with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HotelIdentityDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "IdentityService",
    "Audience": "HotelManagement",
    "ExpiryMinutes": 60
  }
}
```

## Database Setup

1. Update the connection string in `appsettings.json`
2. Apply the migration:

```bash
dotnet ef database update
```

## Running the Application

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or the port configured in launchSettings.json).

Swagger UI is available at the root URL: `https://localhost:5001`

## API Endpoints

### Register a new user

```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "password123"
}
```

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "password123"
}
```

### Get current user (requires authentication)

```http
GET /api/auth/me
Authorization: Bearer <your-jwt-token>
```

## Testing with Swagger

1. Run the application
2. Navigate to the Swagger UI (root URL)
3. Register a new user using `/api/auth/register`
4. Login using `/api/auth/login` to get a JWT token
5. Click the **Authorize** button in Swagger
6. Enter `Bearer <your-token>` in the value field
7. Test the protected `/api/auth/me` endpoint

## User Roles

- **Guest**: Default role assigned during registration
- **Staff**: Hotel staff member
- **Manager**: Hotel manager
- **Admin**: System administrator

Note: Only Guest role is assignable during normal registration. Other roles must be assigned through database updates or admin endpoints (to be implemented).
