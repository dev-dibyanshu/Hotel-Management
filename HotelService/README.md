# HotelService

Hotel operations microservice for Hotel Management System.

## Overview

This service handles the main hotel operations including guests, room types, rooms, and reservations.

## Architecture

Simple layered architecture with the following structure:

```
Controller → Service → Repository → DbContext → SQL Server
```

## Features

- **Guest Management**: Create, read, update, and delete guests
- **Room Type Management**: Manage different room types (Single, Double, Suite, etc.)
- **Room Management**: Manage individual rooms with availability tracking
- **Reservation Management**: Create, update, cancel, and track reservations
- **Available Rooms**: Query available rooms for booking

## Tech Stack

- ASP.NET Core 8.0 Web API
- Entity Framework Core 8.0
- SQL Server
- JWT Bearer Authentication (tokens from IdentityService)
- Swagger/OpenAPI

## Project Structure

```
HotelService/
├── Controllers/          # API endpoints
│   ├── GuestsController.cs
│   ├── RoomTypesController.cs
│   ├── RoomsController.cs
│   └── ReservationsController.cs
├── DTOs/                # Data transfer objects
├── Data/                # DbContext
├── Migrations/          # EF Core migrations
├── Models/              # Domain models
│   ├── Guest.cs
│   ├── RoomType.cs
│   ├── Room.cs
│   ├── Reservation.cs
│   └── ReservationStatus.cs
├── Repositories/        # Data access layer
├── Services/            # Business logic
└── Program.cs           # Application entry point
```

## Configuration

Update `appsettings.json` with your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "same-secret-key-as-IdentityService",
    "Issuer": "IdentityService",
    "Audience": "HotelManagement",
    "ExpiryMinutes": 60
  }
}
```

**Important**: The JWT configuration must match the IdentityService configuration for tokens to work correctly.

## Database Setup

1. Update the connection string in `appsettings.json`
2. Apply the migration:

```bash
dotnet ef database update
```

This creates the following tables:
- Guests
- RoomTypes
- Rooms
- Reservations

## Running the Application

```bash
dotnet run
```

The API will be available at `https://localhost:7222` (or the port configured in launchSettings.json).

Swagger UI is available at the root URL.

## API Endpoints

### Guests

```http
GET    /api/guests          # Get all guests
GET    /api/guests/{id}     # Get guest by ID
POST   /api/guests          # Create new guest
PUT    /api/guests/{id}     # Update guest
DELETE /api/guests/{id}     # Delete guest
```

### Room Types

```http
GET    /api/roomtypes       # Get all room types
GET    /api/roomtypes/{id}  # Get room type by ID
POST   /api/roomtypes       # Create new room type
PUT    /api/roomtypes/{id}  # Update room type
DELETE /api/roomtypes/{id}  # Delete room type
```

### Rooms

```http
GET    /api/rooms           # Get all rooms
GET    /api/rooms/available # Get only available rooms
GET    /api/rooms/{id}      # Get room by ID
POST   /api/rooms           # Create new room
PUT    /api/rooms/{id}      # Update room
DELETE /api/rooms/{id}      # Delete room
```

### Reservations

```http
GET    /api/reservations          # Get all reservations
GET    /api/reservations/{id}     # Get reservation by ID
POST   /api/reservations          # Create new reservation
PUT    /api/reservations/{id}     # Update reservation
POST   /api/reservations/{id}/cancel  # Cancel reservation
DELETE /api/reservations/{id}     # Delete reservation
```

## Authentication

All endpoints require JWT Bearer authentication. 

1. First, register and login through the **IdentityService** to get a JWT token
2. In Swagger UI, click **Authorize** and enter: `Bearer <your-token>`
3. All subsequent requests will include the authentication token

## Models

### Guest
- Id, FirstName, LastName, Email, Phone, CreatedAt

### RoomType
- Id, Name, Description, Capacity

### Room
- Id, RoomNumber (unique), RoomTypeId, Rate, IsAvailable

### Reservation
- Id, GuestId, RoomId, CheckInDate, CheckOutDate, Status, CreatedAt

### ReservationStatus (Enum)
- Pending
- Confirmed
- Cancelled
- Completed

## Business Logic

### Creating a Reservation

When creating a reservation, the system:
1. Validates that check-out date is after check-in date
2. Verifies the guest exists
3. Verifies the room exists
4. Checks if the room is available
5. Creates the reservation with status "Pending"
6. Marks the room as unavailable

### Cancelling a Reservation

When cancelling a reservation:
1. Changes reservation status to "Cancelled"
2. Marks the room as available again

## Testing with Swagger

1. Start the IdentityService and get a JWT token
2. Start this HotelService
3. Navigate to Swagger UI
4. Click **Authorize** and enter your token
5. Create a room type (e.g., "Single", capacity 1)
6. Create a room (e.g., room number "101")
7. Create a guest
8. Create a reservation linking the guest and room
9. Check that the room is now unavailable
10. Cancel the reservation
11. Check that the room is available again

## Relationships

```
RoomType (1) → (Many) Rooms
Guest (1) → (Many) Reservations
Room (1) → (Many) Reservations
```

## What's NOT Included

This service focuses on core hotel operations only. It does NOT include:
- Housekeeping management
- Restaurant/dining services
- Room service
- Inventory management
- Notifications
- Reviews/ratings
- Loyalty programs
- Promotions/discounts
- Payment processing
- Payroll
- Employee management

Those features may be added in future services or versions.
