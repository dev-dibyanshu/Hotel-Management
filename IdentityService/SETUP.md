# IdentityService Setup Guide

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, or Full)
- A code editor (Visual Studio, VS Code, or Rider)

## Step-by-Step Setup

### 1. Configure Database Connection

Open `appsettings.json` or `appsettings.Development.json` and update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=HotelIdentityDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**Connection String Examples:**

For SQL Server with Windows Authentication:
```
Server=localhost;Database=HotelIdentityDb;Trusted_Connection=True;TrustServerCertificate=True;
```

For SQL Server with username/password:
```
Server=localhost;Database=HotelIdentityDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

For LocalDB:
```
Server=(localdb)\\mssqllocaldb;Database=HotelIdentityDb;Trusted_Connection=True;TrustServerCertificate=True;
```

### 2. Update JWT Secret Key (Important for Production)

In `appsettings.json`, update the JWT key to a strong secret:

```json
"Jwt": {
  "Key": "your-super-secret-key-at-least-32-characters-long",
  "Issuer": "IdentityService",
  "Audience": "HotelManagement",
  "ExpiryMinutes": 60
}
```

**Note**: The key must be at least 32 characters for HS256 algorithm.

### 3. Apply Database Migrations

Run the following command to create the database and apply migrations:

```bash
dotnet ef database update
```

This will:
- Create the `HotelIdentityDb` database
- Create the `Users` table with the required schema

### 4. Run the Application

Start the application:

```bash
dotnet run
```

The application will start on:
- HTTPS: `https://localhost:7011`
- HTTP: `http://localhost:5038`

### 5. Access Swagger UI

Open your browser and navigate to:

```
https://localhost:7011
```

or

```
http://localhost:5038
```

You should see the Swagger UI with three endpoints:
- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`

## Testing the API

### 1. Register a New User

In Swagger UI, expand the `POST /api/auth/register` endpoint:

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "password123"
}
```

**Expected Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "role": "Guest"
}
```

### 2. Login with Existing User

Expand the `POST /api/auth/login` endpoint:

**Request Body:**
```json
{
  "email": "john@example.com",
  "password": "password123"
}
```

**Expected Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "role": "Guest"
}
```

Copy the `token` value (the long string).

### 3. Test Protected Endpoint

1. Click the **Authorize** button at the top of Swagger UI
2. In the dialog, enter: `Bearer <your-token-here>`
   - Example: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
3. Click **Authorize**, then **Close**
4. Expand the `GET /api/auth/me` endpoint
5. Click **Try it out**, then **Execute**

**Expected Response (200 OK):**
```json
{
  "userId": "1",
  "email": "john@example.com",
  "role": "Guest"
}
```

## Common Issues

### Issue: Database connection fails

**Solution**: 
- Verify SQL Server is running
- Check connection string is correct
- Ensure the user has permissions to create databases

### Issue: JWT token validation fails

**Solution**:
- Ensure the JWT key in `appsettings.json` is at least 32 characters
- Check that the token is prefixed with "Bearer " in the Authorization header
- Verify the token hasn't expired (default: 60 minutes)

### Issue: Migration fails

**Solution**:
```bash
# Remove existing migration
dotnet ef migrations remove

# Create new migration
dotnet ef migrations add InitialCreate

# Apply migration
dotnet ef database update
```

## Next Steps

After successful setup:

1. Test all three endpoints using Swagger
2. Verify the database has been created with the Users table
3. Try registering multiple users
4. Test invalid scenarios (wrong password, duplicate email, etc.)
5. Review the code structure and understand the flow

## Database Schema

The `Users` table has the following columns:

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| FirstName | nvarchar(100) | Required |
| LastName | nvarchar(100) | Required |
| Email | nvarchar(255) | Required, Unique |
| PasswordHash | nvarchar(max) | Required |
| Role | nvarchar(max) | Required, Default: "Guest" |
| CreatedAt | datetime2 | Required, Default: GETUTCDATE() |
| IsActive | bit | Required, Default: 1 |

## Security Notes

- Passwords are hashed using ASP.NET Core's `PasswordHasher<T>`
- JWT tokens are signed with HMAC-SHA256
- All endpoints use HTTPS in production
- Email is case-sensitive for uniqueness
- Default role is "Guest" (cannot choose Admin during registration)
