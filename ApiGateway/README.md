# API Gateway

The API Gateway provides a single entry point for all microservices in the Hotel Management System using Ocelot.

## Overview

This gateway routes client requests to the appropriate downstream microservice:

- **IdentityService** (Port 7011) - Authentication and user management
- **HotelService** (Port 7264) - Hotel operations (guests, rooms, reservations)
- **ManagementService** (Port 7001) - Management operations (staff, inventory, bills, payments)

## Configuration

The gateway is configured via `ocelot.json` which defines:
- Downstream services (target microservices)
- Upstream routes (gateway endpoints)
- Port mappings
- HTTP methods

## Gateway Port

- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`

## Route Mapping

All routes follow the pattern: `/gateway/{service}/{resource}`

### IdentityService Routes

| Upstream (Gateway) | Downstream (Service) | Method | Description |
|-------------------|---------------------|---------|-------------|
| `/gateway/auth/register` | `/api/Auth/register` | POST | Register new user |
| `/gateway/auth/login` | `/api/Auth/login` | POST | User login |
| `/gateway/auth/me` | `/api/Auth/me` | GET | Get current user |

### HotelService Routes

**Guests:**
- `/gateway/guests` → `/api/Guests` (GET, POST)
- `/gateway/guests/{id}` → `/api/Guests/{id}` (GET, PUT, DELETE)

**Room Types:**
- `/gateway/roomtypes` → `/api/RoomTypes` (GET, POST)
- `/gateway/roomtypes/{id}` → `/api/RoomTypes/{id}` (GET, PUT, DELETE)

**Rooms:**
- `/gateway/rooms` → `/api/Rooms` (GET, POST)
- `/gateway/rooms/{id}` → `/api/Rooms/{id}` (GET, PUT, DELETE)
- `/gateway/rooms/available` → `/api/Rooms/available` (GET)

**Reservations:**
- `/gateway/reservations` → `/api/Reservations` (GET, POST)
- `/gateway/reservations/{id}` → `/api/Reservations/{id}` (GET, PUT, DELETE)
- `/gateway/reservations/{id}/cancel` → `/api/Reservations/{id}/cancel` (POST)

### ManagementService Routes

**Staff:**
- `/gateway/staff` → `/api/Staff` (GET, POST)
- `/gateway/staff/{id}` → `/api/Staff/{id}` (GET, PUT, DELETE)

**Departments:**
- `/gateway/departments` → `/api/Departments` (GET, POST)
- `/gateway/departments/{id}` → `/api/Departments/{id}` (GET, PUT, DELETE)

**Inventory:**
- `/gateway/inventory` → `/api/Inventory` (GET, POST)
- `/gateway/inventory/{id}` → `/api/Inventory/{id}` (GET, PUT, DELETE)
- `/gateway/inventory/{id}/quantity` → `/api/Inventory/{id}/quantity` (PATCH)

**Bills:**
- `/gateway/bills` → `/api/Bills` (GET, POST)
- `/gateway/bills/{id}` → `/api/Bills/{id}` (GET, PUT, DELETE)

**Payments:**
- `/gateway/payments` → `/api/Payments` (GET, POST)
- `/gateway/payments/{id}` → `/api/Payments/{id}` (GET)
- `/gateway/payments/bill/{billId}` → `/api/Payments/bill/{billId}` (GET)

## Running the Gateway

1. Ensure all three microservices are running on their configured ports
2. Run the API Gateway:

```bash
dotnet run
```

3. Access the gateway at `https://localhost:7000`

## Example Usage

Instead of calling microservices directly:
```
POST https://localhost:7011/api/Auth/login
```

Call through the gateway:
```
POST https://localhost:7000/gateway/auth/login
```

## Authentication

The gateway forwards JWT tokens from the Authorization header to downstream services. Authenticated endpoints require a valid Bearer token obtained from `/gateway/auth/login`.

Example:
```bash
# Login
curl -X POST https://localhost:7000/gateway/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password"}'

# Use returned token
curl -X GET https://localhost:7000/gateway/auth/me \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Technology Stack

- **.NET 8.0**
- **Ocelot 23.3.3** - API Gateway framework
