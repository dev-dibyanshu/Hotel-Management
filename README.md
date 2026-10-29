# Hotel Management System

A microservices-based hotel management system built with ASP.NET Core 8.0, Entity Framework Core, and SQL Server.

## Architecture

This project follows a microservices architecture with simple, layered design inside each service:

```
Controller → Service → Repository → DbContext → SQL Server
```

## Microservices

### 1. IdentityService
Authentication and user management service.

**Features:**
- User registration
- User login
- JWT token generation
- Role-based user management (Guest, Staff, Manager, Admin)

**Port:** `https://localhost:7011`

[View IdentityService Documentation](./IdentityService/README.md)

### 2. HotelService
Core hotel operations management service.

**Features:**
- Guest management (CRUD)
- Room type management (Single, Double, Suite, etc.)
- Room management with availability tracking
- Reservation management
- Reservation cancellation
- Available rooms query

**Port:** `https://localhost:7264`

[View HotelService Documentation](./HotelService/README.md)

## Technology Stack

- **Framework:** ASP.NET Core 8.0 Web API
- **ORM:** Entity Framework Core 8.0
- **Database:** SQL Server
- **Authentication:** JWT Bearer Tokens
- **API Documentation:** Swagger/OpenAPI

## Project Structure

```
Hotel-Management/
├── IdentityService/          # Authentication microservice
│   ├── Controllers/
│   ├── DTOs/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── Migrations/
│   └── README.md
├── HotelService/             # Hotel operations microservice
│   ├── Controllers/
│   ├── DTOs/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── Migrations/
│   └── README.md
└── README.md
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, or Full)
- A code editor (Visual Studio, VS Code, or Rider)

### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/dev-dibyanshu/Hotel-Managerment.git
   cd Hotel-Managerment
   ```

2. **Configure IdentityService:**
   ```bash
   cd IdentityService
   ```
   - Update `appsettings.json` with your SQL Server connection string
   - Update JWT configuration
   - Apply migrations: `dotnet ef database update`
   - Run: `dotnet run`

3. **Configure HotelService:**
   ```bash
   cd ../HotelService
   ```
   - Update `appsettings.json` with your SQL Server connection string
   - Ensure JWT configuration matches IdentityService
   - Apply migrations: `dotnet ef database update`
   - Run: `dotnet run`

### Quick Start Guide

1. **Start IdentityService** (Port: 7011)
2. **Register a user** via `POST /api/auth/register`
3. **Login** to get JWT token via `POST /api/auth/login`
4. **Start HotelService** (Port: 7264)
5. **Authorize in Swagger** with the JWT token
6. **Test hotel operations** (guests, rooms, reservations)

## API Documentation

Each service has Swagger UI available at their root URL:

- **IdentityService:** `https://localhost:7011`
- **HotelService:** `https://localhost:7264`

## Authentication Flow

1. User registers/logs in through **IdentityService**
2. IdentityService returns a JWT token
3. User includes JWT token in requests to **HotelService**
4. HotelService validates the token using shared JWT configuration
5. Authorized users can access hotel management endpoints

## Database Schema

### IdentityService Database (HotelIdentityDb)

- **Users:** User accounts with email, password hash, and roles

### HotelService Database (HotelManagementDb)

- **Guests:** Hotel guest information
- **RoomTypes:** Different room categories
- **Rooms:** Individual rooms with availability status
- **Reservations:** Booking information linking guests and rooms

## Key Features

### Simple & Maintainable
- No over-engineering
- Clear separation of concerns
- Straightforward code structure
- Standard ASP.NET Core patterns

### Secure
- JWT-based authentication
- Password hashing with ASP.NET Identity
- Role-based authorization ready
- Protected endpoints

### Well Documented
- Comprehensive README files
- SETUP guides for each service
- Inline code comments
- Swagger API documentation

## Development Approach

This project intentionally avoids over-engineering:

- ❌ No Clean Architecture layers
- ❌ No CQRS or MediatR
- ❌ No Generic Repository
- ❌ No AutoMapper
- ❌ No custom result/response wrappers
- ✅ Simple, readable, maintainable code
- ✅ Standard Repository Pattern
- ✅ Clear service layer
- ✅ Direct DTO mapping

## Testing

Use Swagger UI for testing:

1. **IdentityService:**
   - Test registration and login
   - Verify JWT token generation

2. **HotelService:**
   - Create room types and rooms
   - Add guests
   - Make reservations
   - Test room availability
   - Cancel reservations

## Detailed Setup Guides

- [IdentityService Setup Guide](./IdentityService/SETUP.md)
- [HotelService Setup Guide](./HotelService/SETUP.md)

## Contributing

This is an educational/demonstration project. Feel free to fork and modify as needed.

## License

This project is for educational purposes.

## Contact

For questions or issues, please open an issue on GitHub.
