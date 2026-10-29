# ManagementService Setup Guide

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

The `Key`, `Issuer`, and `Audience` values must be identical to IdentityService for token validation to work.

### 3. Apply Database Migrations

Run the following command to create the database:

```bash
dotnet ef database update
```

This will:
- Create the `HotelManagementDb` database
- Create tables: Departments, Staff, InventoryItems, Bills, Payments
- Set up all foreign key relationships

### 4. Run the Application

Start the service:

```bash
dotnet run
```

The application will start on configured ports (check `Properties/launchSettings.json`).

### 5. Access Swagger UI

Open your browser and navigate to the root URL (e.g., `https://localhost:7xxx`).

You should see the Swagger UI with five main sections:
- Departments
- Staff
- Inventory
- Bills
- Payments

## Getting a JWT Token

Before testing ManagementService endpoints, you need a JWT token from IdentityService:

1. Start the **IdentityService**
2. Register a new user or login with existing credentials
3. Copy the JWT token from the response
4. Return to ManagementService Swagger UI
5. Click the **Authorize** button
6. Enter: `Bearer <your-token>`
7. Click **Authorize**, then **Close**

Now all endpoints will include your authentication token.

## Testing the API (Complete Workflow)

### Step 1: Create Departments

Endpoint: `POST /api/departments`

```json
{
  "name": "Reception",
  "description": "Front desk and guest services"
}
```

Create more departments:

```json
{
  "name": "Housekeeping",
  "description": "Room cleaning and maintenance"
}
```

```json
{
  "name": "Management",
  "description": "Hotel management and administration"
}
```

### Step 2: Create Staff Members

Endpoint: `POST /api/staff`

**Note**: You need Admin or Manager role for this operation.

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@hotel.com",
  "phone": "+1234567890",
  "departmentId": 1,
  "role": "Receptionist",
  "salary": 35000
}
```

Create more staff:

```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane.smith@hotel.com",
  "phone": "+1234567891",
  "departmentId": 2,
  "role": "Housekeeper",
  "salary": 30000
}
```

### Step 3: Add Inventory Items

Endpoint: `POST /api/inventory`

```json
{
  "name": "Bedsheets",
  "description": "White cotton bedsheets - King size",
  "quantity": 100,
  "unitPrice": 25.00
}
```

Add more items:

```json
{
  "name": "Towels",
  "description": "Bath towels - White",
  "quantity": 200,
  "unitPrice": 10.00
}
```

```json
{
  "name": "Toiletries",
  "description": "Shampoo, soap, and amenities kit",
  "quantity": 150,
  "unitPrice": 5.00
}
```

### Step 4: Update Inventory Quantity

Endpoint: `PUT /api/inventory/{id}/quantity`

```json
{
  "quantity": 95
}
```

This quickly updates quantity without modifying other fields.

### Step 5: Create a Bill

Endpoint: `POST /api/bills`

**Note**: ReservationId should be a valid reservation ID from HotelService.

```json
{
  "reservationId": 1,
  "amount": 500.00
}
```

**Response:**
- Status: 201 Created
- Bill created with status "Pending"

### Step 6: Create a Payment

Endpoint: `POST /api/payments`

```json
{
  "billId": 1,
  "amount": 500.00,
  "paymentMethod": "Card"
}
```

**Expected Behavior:**
- Payment is created with status "Completed"
- Bill status automatically updates to "Paid" (if payment >= bill amount)

### Step 7: Verify Bill Status

Endpoint: `GET /api/bills/1`

Should show `"status": "Paid"`

## Common Scenarios

### Partial Payment

Create a payment less than the bill amount:

```json
{
  "billId": 2,
  "amount": 250.00,
  "paymentMethod": "Cash"
}
```

The bill will remain "Pending" until total payments >= bill amount.

### Getting All Staff by Department

1. `GET /api/staff` - Returns all staff with department information
2. Filter on the client side by `departmentId` or `departmentName`

### Updating Staff Information

Endpoint: `PUT /api/staff/{id}`

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@hotel.com",
  "phone": "+1234567890",
  "departmentId": 1,
  "role": "Senior Receptionist",
  "salary": 40000
}
```

## Validation Rules

### Department
- Name: Required

### Staff
- First name: Required
- Last name: Required
- Email: Required, valid email format
- Phone: Required
- Department: Must exist
- Role: Required
- Salary: Cannot be negative

### Inventory Item
- Name: Required
- Quantity: Cannot be negative
- Unit price: Cannot be negative

### Bill
- ReservationId: Required
- Amount: Must be greater than 0

### Payment
- BillId: Required (must exist)
- Amount: Must be greater than 0
- Payment method: Required

## Role-Based Authorization

### Admin Role
- Full access to all operations
- Can delete departments, staff, bills, inventory

### Manager Role
- Can manage departments, staff, bills, inventory
- Cannot delete (except staff can be managed)

### Staff Role
- Can manage inventory, bills, and payments
- Read-only for departments and staff

### Guest Role
- Read-only access to non-sensitive data (if implemented)

## Common Issues

### Issue: 401 Unauthorized on all endpoints

**Solution**: 
- Ensure you have a valid JWT token from IdentityService
- Verify JWT configuration matches IdentityService
- Check token is entered correctly: `Bearer <token>`
- Ensure token hasn't expired (default: 60 minutes)

### Issue: 403 Forbidden on certain endpoints

**Solution**:
- Check your user role in IdentityService
- Some endpoints require Admin or Manager roles
- Login with appropriate credentials

### Issue: Cannot create staff - Department not found

**Solution**:
- First create departments using `POST /api/departments`
- Use the returned department ID when creating staff

### Issue: Database connection fails

**Solution**: 
- Verify SQL Server is running
- Check connection string is correct
- Ensure user has permissions to create databases

### Issue: Bill status not updating to Paid

**Solution**:
- Verify payment amount >= bill amount
- Check payment was created successfully
- Query the bill again to see updated status

## Database Schema

### Departments Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| Name | nvarchar(100) | Required |
| Description | nvarchar(500) | |

### Staff Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| FirstName | nvarchar(100) | Required |
| LastName | nvarchar(100) | Required |
| Email | nvarchar(255) | Required |
| Phone | nvarchar(20) | Required |
| DepartmentId | int | Foreign Key to Departments |
| Role | nvarchar(100) | Required |
| Salary | decimal(18,2) | Required |
| IsActive | bit | Default: 1 (true) |
| CreatedAt | datetime2 | Default: GETUTCDATE() |

### InventoryItems Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| Name | nvarchar(200) | Required |
| Description | nvarchar(500) | |
| Quantity | int | Required |
| UnitPrice | decimal(18,2) | Required |
| CreatedAt | datetime2 | Default: GETUTCDATE() |

### Bills Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| ReservationId | int | Required (NOT a foreign key) |
| Amount | decimal(18,2) | Required |
| BillDate | datetime2 | Required |
| Status | nvarchar(max) | Required (Pending/Paid/Cancelled) |

### Payments Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| BillId | int | Foreign Key to Bills |
| Amount | decimal(18,2) | Required |
| PaymentDate | datetime2 | Required |
| PaymentMethod | nvarchar(max) | Required (Cash/Card/UPI/Online) |
| Status | nvarchar(50) | Required |

## Microservice Architecture

ManagementService is independent:
- Has its own database (`HotelManagementDb`)
- Does NOT directly access HotelService or IdentityService databases
- `Bill.ReservationId` is just an ID reference (not a foreign key)
- Maintains proper microservice boundaries

## Integration with Other Services

```
IdentityService (Authentication)
        ↓
   JWT Token
        ↓
ManagementService (validates token)
```

HotelService and ManagementService are independent but can share reservation IDs:
- HotelService creates reservations
- ManagementService creates bills using those reservation IDs
- No direct database coupling

## Next Steps

After successful setup:

1. Test all CRUD operations for departments
2. Create staff members in different departments
3. Add inventory items
4. Create bills for reservations
5. Process payments and verify bill status updates
6. Test role-based authorization
7. Verify JWT authentication works correctly

## Complete Testing Flow

1. **Setup**:
   - Start IdentityService (port 7011)
   - Start HotelService (port 7264)
   - Start ManagementService (port 7xxx)

2. **Authentication**:
   - Register/Login via IdentityService
   - Get JWT token
   - Authorize in ManagementService Swagger

3. **Create Master Data**:
   - Create departments
   - Create staff
   - Add inventory items

4. **Billing Workflow**:
   - Create reservation in HotelService (get reservation ID)
   - Create bill in ManagementService (use reservation ID)
   - Process payment
   - Verify bill status updated to Paid

This completes the end-to-end testing of the ManagementService!
