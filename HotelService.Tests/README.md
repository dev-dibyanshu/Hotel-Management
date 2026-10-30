# HotelService.Tests

Unit tests for the HotelService microservice using NUnit and Moq.

## Overview

This test project contains unit tests for the **ReservationService** class, which handles reservation business logic including validation, room availability checking, and reservation cancellation.

## Technology Stack

- **Testing Framework:** NUnit 3.14.0
- **Mocking Framework:** Moq 4.20.72
- **Test Adapter:** NUnit3TestAdapter 4.5.0
- **Test SDK:** Microsoft.NET.Test.Sdk 17.11.1
- **.NET Version:** .NET 8.0

## Test Class

### ReservationServiceTests

Tests the **ReservationService** business logic without requiring a database connection.

#### Test Cases (9 total)

1. **CreateAsync_WhenValidDataAndRoomAvailable_ReturnsReservationDto**
   - Verifies successful reservation creation when all validations pass
   - Checks that room is marked as unavailable after reservation
   - Validates correct DTO mapping

2. **CreateAsync_WhenCheckOutDateBeforeCheckInDate_ReturnsNull**
   - Tests date validation logic
   - Ensures check-out date must be after check-in date

3. **CreateAsync_WhenGuestDoesNotExist_ReturnsNull**
   - Tests guest existence validation
   - Ensures reservation cannot be created for non-existent guest

4. **CreateAsync_WhenRoomDoesNotExist_ReturnsNull**
   - Tests room existence validation
   - Ensures reservation cannot be created for non-existent room

5. **CreateAsync_WhenRoomIsNotAvailable_ReturnsNull**
   - Tests room availability validation
   - Ensures reservation cannot be created for unavailable rooms

6. **CancelAsync_WhenReservationExists_UpdatesStatusAndMakesRoomAvailable**
   - Verifies reservation cancellation logic
   - Checks that reservation status is updated to Cancelled
   - Validates that room is marked as available after cancellation

7. **CancelAsync_WhenReservationDoesNotExist_ReturnsFalse**
   - Tests cancellation of non-existent reservation
   - Ensures proper error handling

8. **GetByIdAsync_WhenReservationExists_ReturnsReservationDto**
   - Tests retrieval of existing reservation
   - Validates correct DTO mapping with guest and room information

9. **GetByIdAsync_WhenReservationDoesNotExist_ReturnsNull**
   - Tests retrieval of non-existent reservation
   - Ensures proper null handling

## Running the Tests

### Run all tests:
```bash
dotnet test
```

### Run with detailed output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Run specific test:
```bash
dotnet test --filter "FullyQualifiedName~CreateAsync_WhenValidDataAndRoomAvailable"
```

## Test Structure

Each test follows the **Arrange-Act-Assert (AAA)** pattern:

```csharp
[Test]
public async Task TestName()
{
    // Arrange - Setup test data and mocks
    var createDto = new CreateReservationDto { ... };
    _mockRepository.Setup(...).ReturnsAsync(...);

    // Act - Execute the method being tested
    var result = await _service.CreateAsync(createDto);

    // Assert - Verify the expected outcome
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Status, Is.EqualTo(expected));
}
```

## Mocking Strategy

The tests use **Moq** to mock repository dependencies:

- **IReservationRepository** - Mocked for reservation data access
- **IGuestRepository** - Mocked for guest validation
- **IRoomRepository** - Mocked for room validation and updates

This approach ensures:
- Tests run without database connection
- Tests are fast and isolated
- Each test is independent

## Test Results

```
Test Run Successful.
Total tests: 9
     Passed: 9
 Total time: < 1 second
```

## Coverage

The tests cover the following business logic scenarios:
- ✅ Successful reservation creation
- ✅ Date validation (check-out after check-in)
- ✅ Guest existence validation
- ✅ Room existence validation
- ✅ Room availability validation
- ✅ Reservation cancellation with room update
- ✅ Handling non-existent reservations
- ✅ Data retrieval and DTO mapping

## Best Practices Demonstrated

1. **Test Isolation** - Each test is independent with fresh mocks
2. **Arrange-Act-Assert** - Clear test structure
3. **Meaningful Test Names** - Describes what is tested and expected outcome
4. **No Database** - Tests use mocks, not real database
5. **Setup Method** - Common initialization in `[SetUp]` method
6. **Async Testing** - Proper async/await test patterns
7. **Verification** - Uses `Verify()` to ensure important interactions occur

## Notes

- These tests do NOT modify production code
- Tests focus on business logic, not infrastructure
- Repository implementations are mocked
- No Entity Framework or SQL Server required to run tests
