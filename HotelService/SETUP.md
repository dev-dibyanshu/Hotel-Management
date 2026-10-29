# HotelService Setup Guide

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, or Full)
- IdentityService running (for JWT authentication)
- A code editor (Visual Studio, VS Code, or Rider)

## Step-by-Step Setup

### 1. Configure Database Connection

Open `appsettings.json` and update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**Connection String Examples:**

For SQL Server with Windows Authentication:
```
Server=localhost;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;
```

For SQL Server with username/password:
```
Server=localhost;Database=HotelManagementDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

For LocalDB:
```
Server=(localdb)\\mssqllocaldb;Database=HotelManagementDb;Trusted_Connection=True;TrustServerCertificate=True;
```

### 2. Configure JWT Settings

**IMPORTANT**: The JWT configuration MUST match your IdentityService settings exactly.

In `appsettings.json`:

```json
"Jwt": {
  "Key": "your-super-secret-key-at-least-32-characters-long",
  "Issuer": "IdentityService",
  "Audience": "HotelManagement",
  "ExpiryMinutes": 60
}
```

The `Key`, `Issuer`, and `Audience` values must be identical to the ones in your IdentityService for tokens to be validated correctly.

### 3. Apply Database Migrations

Run the following command to create the database:

```bash
dotnet ef database update
```

This will:
- Create the `HotelManagementDb` database
- Create tables: Guests, RoomTypes, Rooms, Reservations
- Set up all foreign key relationships

### 4. Run the Application

Start the service:

```bash
dotnet run
```

The application will start on:
- HTTPS: `https://localhost:7264`
- HTTP: `http://localhost:5029`

### 5. Access Swagger UI

Open your browser and navigate to:

```
https://localhost:7264
```

You should see the Swagger UI with four main sections:
- Guests
- RoomTypes
- Rooms
- Reservations

## Getting a JWT Token

Before you can test HotelService endpoints, you need a JWT token from IdentityService:

1. Start the **IdentityService**
2. Register a new user or login with existing credentials
3. Copy the JWT token from the response
4. Return to HotelService Swagger UI
5. Click the **Authorize** button
6. Enter: `Bearer <your-token>`
7. Click **Authorize**, then **Close**

Now all endpoints will include your authentication token.

## Testing the API (Complete Workflow)

### Step 1: Create a Room Type

Endpoint: `POST /api/roomtypes`

```json
{
  "name": "Single",
  "description": "Single occupancy room with one bed",
  "capacity": 1
}
```

**Response:** Note the returned `id` (e.g., `1`)

### Step 2: Create Another Room Type

```json
{
  "name": "Double",
  "description": "Double occupancy room with two beds",
  "capacity": 2
}
```

### Step 3: Create Rooms

Endpoint: `POST /api/rooms`

```json
{
  "roomNumber": "101",
  "roomTypeId": 1,
  "rate": 99.99
}
```

Create a few more:

```json
{
  "roomNumber": "102",
  "roomTypeId": 1,
  "rate": 99.99
}
```

```json
{
  "roomNumber": "201",
  "roomTypeId": 2,
  "rate": 149.99
}
```

### Step 4: Check Available Rooms

Endpoint: `GET /api/rooms/available`

All rooms should show as available with `"isAvailable": true`

### Step 5: Create a Guest

Endpoint: `POST /api/guests`

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "+1234567890"
}
```

**Response:** Note the `id` (e.g., `1`)

### Step 6: Create a Reservation

Endpoint: `POST /api/reservations`

```json
{
  "guestId": 1,
  "roomId": 1,
  "checkInDate": "2026-11-01T14:00:00",
  "checkOutDate": "2026-11-05T11:00:00"
}
```

**Expected Response:**
- Status: 201 Created
- Reservation created with status "Pending"
- Room marked as unavailable

### Step 7: Verify Room Unavailability

Endpoint: `GET /api/rooms/available`

Room 101 should no longer appear in the available rooms list.

Endpoint: `GET /api/rooms/1`

Should show `"isAvailable": false`

### Step 8: Cancel the Reservation

Endpoint: `POST /api/reservations/1/cancel`

**Expected Response:**
- Status: 200 OK
- Message: "Reservation cancelled successfully"

### Step 9: Verify Room is Available Again

Endpoint: `GET /api/rooms/available`

Room 101 should now appear as available.

Endpoint: `GET /api/reservations/1`

Should show `"status": "Cancelled"`

## Common Scenarios

### Creating Multiple Reservations

You can create multiple reservations for the same guest:

```json
{
  "guestId": 1,
  "roomId": 2,
  "checkInDate": "2026-11-10T14:00:00",
  "checkOutDate": "2026-11-12T11:00:00"
}
```

### Updating a Reservation

Endpoint: `PUT /api/reservations/{id}`

You can update the check-in and check-out dates.

### Getting All Guests

Endpoint: `GET /api/guests`

Returns all registered guests.

### Getting All Reservations

Endpoint: `GET /api/reservations`

Returns all reservations with guest names and room numbers.

## Validation Rules

### Guest
- First name: Required
- Last name: Required
- Email: Required, must be valid email format
- Phone: Required

### Room Type
- Name: Required
- Capacity: Required, must be between 1 and 10

### Room
- Room number: Required
- Room type: Must exist
- Rate: Required, must be greater than 0

### Reservation
- Guest: Must exist
- Room: Must exist
- Room must be available
- Check-in date: Required
- Check-out date: Required, must be after check-in date

## Common Issues

### Issue: 401 Unauthorized on all endpoints

**Solution**: 
- Ensure you have a valid JWT token from IdentityService
- Check that the JWT configuration matches IdentityService
- Verify the token is entered correctly: `Bearer <token>`
- Ensure the token hasn't expired (default: 60 minutes)

### Issue: Cannot create reservation - room not available

**Solution**:
- Check if the room is already reserved
- If needed, cancel the existing reservation
- Verify the room exists using `GET /api/rooms/{id}`

### Issue: Database connection fails

**Solution**: 
- Verify SQL Server is running
- Check connection string is correct
- Ensure the user has permissions to create databases

### Issue: Cannot create room - room type not found

**Solution**:
- First create a room type using `POST /api/roomtypes`
- Use the returned ID when creating the room

## Database Schema

### Guests Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| FirstName | nvarchar(100) | Required |
| LastName | nvarchar(100) | Required |
| Email | nvarchar(255) | Required |
| Phone | nvarchar(20) | Required |
| CreatedAt | datetime2 | Default: GETUTCDATE() |

### RoomTypes Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| Name | nvarchar(100) | Required |
| Description | nvarchar(500) | |
| Capacity | int | Required |

### Rooms Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| RoomNumber | nvarchar(10) | Required, Unique |
| RoomTypeId | int | Foreign Key to RoomTypes |
| Rate | decimal(18,2) | Required |
| IsAvailable | bit | Default: 1 (true) |

### Reservations Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| GuestId | int | Foreign Key to Guests |
| RoomId | int | Foreign Key to Rooms |
| CheckInDate | datetime2 | Required |
| CheckOutDate | datetime2 | Required |
| Status | nvarchar(max) | Required (Pending/Confirmed/Cancelled/Completed) |
| CreatedAt | datetime2 | Default: GETUTCDATE() |

## Next Steps

After successful setup:

1. Test all CRUD operations for guests, room types, and rooms
2. Create reservations and verify room availability changes
3. Test reservation cancellation
4. Verify JWT authentication works correctly
5. Explore the complete workflow end-to-end

## Integration with IdentityService

This service requires IdentityService to be running for authentication. The complete flow:

1. User registers/logs in via IdentityService → receives JWT token
2. User includes JWT token in HotelService requests
3. HotelService validates token using shared JWT configuration
4. Authorized users can manage guests, rooms, and reservations

## Port Configuration

Default ports (configured in `Properties/launchSettings.json`):
- HTTPS: 7264
- HTTP: 5029

To change ports, update the `applicationUrl` in `launchSettings.json`.
