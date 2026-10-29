# ManagementService

Management operations microservice for Hotel Management System.

## Overview

This service handles hotel management operations including staff, departments, inventory, billing, and payments.

## Architecture

Simple layered architecture with the following structure:

```
Controller → Service → Repository → DbContext → SQL Server
```

## Features

- **Staff Management**: Create, read, update, and delete staff members
- **Department Management**: Manage hotel departments (Reception, Housekeeping, etc.)
- **Inventory Management**: Track hotel inventory items (supplies, amenities, etc.)
- **Billing**: Create and manage bills for reservations
- **Payment Processing**: Record payments and update bill status

## Tech Stack

- ASP.NET Core 8.0 Web API
- Entity Framework Core 8.0
- SQL Server
- JWT Bearer Authentication (tokens from IdentityService)
- Swagger/OpenAPI

## Project Structure

```
ManagementService/
├── Controllers/          # API endpoints
│   ├── StaffController.cs
│   ├── DepartmentsController.cs
│   ├── InventoryController.cs
│   ├── BillsController.cs
│   └── PaymentsController.cs
├── DTOs/                # Data transfer objects
├── Data/                # DbContext
├── Migrations/          # EF Core migrations
├── Models/              # Domain models
│   ├── Staff.cs
│   ├── Department.cs
│   ├── InventoryItem.cs
│   ├── Bill.cs
│   ├── Payment.cs
│   ├── BillStatus.cs
│   └── PaymentMethod.cs
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

**Important**: The JWT configuration must match the IdentityService configuration.

## Database Setup

1. Update the connection string in `appsettings.json`
2. Apply the migration:

```bash
dotnet ef database update
```

This creates the following tables:
- Departments
- Staff
- InventoryItems
- Bills
- Payments

## Running the Application

```bash
dotnet run
```

The API will be available at `https://localhost:7xxx` (check launchSettings.json for actual ports).

Swagger UI is available at the root URL.

## API Endpoints

### Departments

```http
GET    /api/departments          # Get all departments
GET    /api/departments/{id}     # Get department by ID
POST   /api/departments          # Create department (Admin/Manager)
PUT    /api/departments/{id}     # Update department (Admin/Manager)
DELETE /api/departments/{id}     # Delete department (Admin)
```

### Staff

```http
GET    /api/staff                # Get all staff
GET    /api/staff/{id}           # Get staff by ID
POST   /api/staff                # Create staff (Admin/Manager)
PUT    /api/staff/{id}           # Update staff (Admin/Manager)
DELETE /api/staff/{id}           # Delete staff (Admin)
```

### Inventory

```http
GET    /api/inventory            # Get all inventory items
GET    /api/inventory/{id}       # Get item by ID
POST   /api/inventory            # Create item (Admin/Manager/Staff)
PUT    /api/inventory/{id}       # Update item (Admin/Manager/Staff)
PUT    /api/inventory/{id}/quantity  # Update quantity (Admin/Manager/Staff)
DELETE /api/inventory/{id}       # Delete item (Admin)
```

### Bills

```http
GET    /api/bills                # Get all bills
GET    /api/bills/{id}           # Get bill by ID
POST   /api/bills                # Create bill (Admin/Manager/Staff)
PUT    /api/bills/{id}           # Update bill (Admin/Manager)
DELETE /api/bills/{id}           # Delete bill (Admin)
```

### Payments

```http
GET    /api/payments             # Get all payments
GET    /api/payments/{id}        # Get payment by ID
POST   /api/payments             # Create payment (Admin/Manager/Staff)
```

## Authentication

All endpoints require JWT Bearer authentication.

1. Get a JWT token from **IdentityService** (register/login)
2. In Swagger UI, click **Authorize** and enter: `Bearer <your-token>`
3. All subsequent requests will include the authentication token

## Authorization

Role-based authorization is implemented:

- **Admin**: Full access to all operations
- **Manager**: Can manage departments, staff, inventory, bills
- **Staff**: Can manage inventory, bills, and payments
- **Guest**: Read-only access (if authenticated)

## Models

### Department
- Id, Name, Description

### Staff
- Id, FirstName, LastName, Email, Phone, DepartmentId, Role, Salary, IsActive, CreatedAt

### InventoryItem
- Id, Name, Description, Quantity, UnitPrice, CreatedAt

### Bill
- Id, ReservationId (from HotelService), Amount, BillDate, Status

### Payment
- Id, BillId, Amount, PaymentDate, PaymentMethod, Status

### BillStatus (Enum)
- Pending, Paid, Cancelled

### PaymentMethod (Enum)
- Cash, Card, UPI, Online

## Microservice Separation

This service maintains its **own database** and does not directly access HotelService or IdentityService databases.

- `Bill.ReservationId` is just an ID reference (not a foreign key to HotelService)
- This maintains proper microservice boundaries

## Business Logic

### Staff Creation
- Validates department exists before creating staff
- Sets default IsActive to true

### Payment Creation
When creating a payment:
1. Validates bill exists
2. Records the payment
3. Updates bill status to "Paid" if payment amount >= bill amount

### Inventory Quantity Update
Provides dedicated endpoint for quick quantity updates without modifying other fields.

## Testing with Swagger

1. Start IdentityService and get a JWT token
2. Start this ManagementService
3. Navigate to Swagger UI
4. Click **Authorize** and enter your token
5. Test the endpoints:
   - Create departments (e.g., "Reception", "Housekeeping")
   - Create staff members
   - Add inventory items
   - Create bills for reservations
   - Record payments

## Database Schema

All tables have proper foreign key relationships:
- Department → Staff (one-to-many)
- Bill → Payment (one-to-many)

No foreign keys to external services (HotelService, IdentityService).

## What's NOT Included

This service focuses on management operations only. It does NOT include:
- Guest management (in HotelService)
- Room management (in HotelService)
- Reservation management (in HotelService)
- User authentication (in IdentityService)
- External payment gateway integration
- Email notifications
- Reporting system
