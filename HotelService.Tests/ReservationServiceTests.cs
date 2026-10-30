using Moq;
using NUnit.Framework;
using HotelService.DTOs;
using HotelService.Models;
using HotelService.Repositories;
using HotelService.Services;

namespace HotelService.Tests;

[TestFixture]
public class ReservationServiceTests
{
    private Mock<IReservationRepository> _reservationRepositoryMock = null!;
    private Mock<IGuestRepository> _guestRepositoryMock = null!;
    private Mock<IRoomRepository> _roomRepositoryMock = null!;
    private ReservationService _reservationService = null!;

    [SetUp]
    public void Setup()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _guestRepositoryMock = new Mock<IGuestRepository>();
        _roomRepositoryMock = new Mock<IRoomRepository>();
        
        _reservationService = new ReservationService(
            _reservationRepositoryMock.Object,
            _guestRepositoryMock.Object,
            _roomRepositoryMock.Object
        );
    }

    [Test]
    public async Task CreateAsync_WhenValidDataAndRoomAvailable_ReturnsReservationDto()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 1,
            RoomId = 1,
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(3)
        };

        var guest = new Guest
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Phone = "1234567890"
        };

        var room = new Room
        {
            Id = 1,
            RoomNumber = "101",
            RoomTypeId = 1,
            Rate = 100.00m,
            IsAvailable = true
        };

        var createdReservation = new Reservation
        {
            Id = 1,
            GuestId = 1,
            RoomId = 1,
            CheckInDate = createDto.CheckInDate,
            CheckOutDate = createDto.CheckOutDate,
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var loadedReservation = new Reservation
        {
            Id = 1,
            GuestId = 1,
            RoomId = 1,
            CheckInDate = createDto.CheckInDate,
            CheckOutDate = createDto.CheckOutDate,
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Guest = guest,
            Room = room
        };

        _guestRepositoryMock
            .Setup(r => r.GetByIdAsync(createDto.GuestId))
            .ReturnsAsync(guest);

        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(createDto.RoomId))
            .ReturnsAsync(room);

        _reservationRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Reservation>()))
            .ReturnsAsync(createdReservation);

        _reservationRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(loadedReservation);

        _roomRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Room>()))
            .ReturnsAsync(room);

        // Act
        var result = await _reservationService.CreateAsync(createDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.GuestId, Is.EqualTo(createDto.GuestId));
        Assert.That(result.RoomId, Is.EqualTo(createDto.RoomId));
        Assert.That(result.Status, Is.EqualTo(ReservationStatus.Pending));
        Assert.That(result.GuestName, Is.EqualTo("John Doe"));
        Assert.That(result.RoomNumber, Is.EqualTo("101"));

        _roomRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Room>(rm => rm.IsAvailable == false)), Times.Once);
    }

    [Test]
    public async Task CreateAsync_WhenCheckOutDateBeforeCheckInDate_ReturnsNull()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 1,
            RoomId = 1,
            CheckInDate = DateTime.UtcNow.AddDays(5),
            CheckOutDate = DateTime.UtcNow.AddDays(2) // Invalid: before check-in
        };

        // Act
        var result = await _reservationService.CreateAsync(createDto);

        // Assert
        Assert.That(result, Is.Null);
        _guestRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _roomRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task CreateAsync_WhenGuestDoesNotExist_ReturnsNull()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 999, // Non-existent guest
            RoomId = 1,
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(3)
        };

        _guestRepositoryMock
            .Setup(r => r.GetByIdAsync(createDto.GuestId))
            .ReturnsAsync((Guest?)null);

        // Act
        var result = await _reservationService.CreateAsync(createDto);

        // Assert
        Assert.That(result, Is.Null);
        _roomRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _reservationRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Reservation>()), Times.Never);
    }

    [Test]
    public async Task CreateAsync_WhenRoomDoesNotExist_ReturnsNull()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 1,
            RoomId = 999, // Non-existent room
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(3)
        };

        var guest = new Guest
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Phone = "1234567890"
        };

        _guestRepositoryMock
            .Setup(r => r.GetByIdAsync(createDto.GuestId))
            .ReturnsAsync(guest);

        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(createDto.RoomId))
            .ReturnsAsync((Room?)null);

        // Act
        var result = await _reservationService.CreateAsync(createDto);

        // Assert
        Assert.That(result, Is.Null);
        _reservationRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Reservation>()), Times.Never);
    }

    [Test]
    public async Task CreateAsync_WhenRoomIsNotAvailable_ReturnsNull()
    {
        // Arrange
        var createDto = new CreateReservationDto
        {
            GuestId = 1,
            RoomId = 1,
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(3)
        };

        var guest = new Guest
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Phone = "1234567890"
        };

        var room = new Room
        {
            Id = 1,
            RoomNumber = "101",
            RoomTypeId = 1,
            Rate = 100.00m,
            IsAvailable = false // Room is not available
        };

        _guestRepositoryMock
            .Setup(r => r.GetByIdAsync(createDto.GuestId))
            .ReturnsAsync(guest);

        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(createDto.RoomId))
            .ReturnsAsync(room);

        // Act
        var result = await _reservationService.CreateAsync(createDto);

        // Assert
        Assert.That(result, Is.Null);
        _reservationRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Reservation>()), Times.Never);
    }

    [Test]
    public async Task CancelAsync_WhenReservationExists_UpdatesStatusAndMakesRoomAvailable()
    {
        // Arrange
        var reservationId = 1;
        var room = new Room
        {
            Id = 1,
            RoomNumber = "101",
            RoomTypeId = 1,
            Rate = 100.00m,
            IsAvailable = false
        };

        var reservation = new Reservation
        {
            Id = reservationId,
            GuestId = 1,
            RoomId = 1,
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(3),
            Status = ReservationStatus.Pending
        };

        _reservationRepositoryMock
            .Setup(r => r.GetByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        _reservationRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
            .ReturnsAsync(reservation);

        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(reservation.RoomId))
            .ReturnsAsync(room);

        _roomRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Room>()))
            .ReturnsAsync(room);

        // Act
        var result = await _reservationService.CancelAsync(reservationId);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(reservation.Status, Is.EqualTo(ReservationStatus.Cancelled));
        
        _reservationRepositoryMock.Verify(
            r => r.UpdateAsync(It.Is<Reservation>(res => res.Status == ReservationStatus.Cancelled)),
            Times.Once
        );
        
        _roomRepositoryMock.Verify(
            r => r.UpdateAsync(It.Is<Room>(rm => rm.IsAvailable == true)),
            Times.Once
        );
    }

    [Test]
    public async Task CancelAsync_WhenReservationDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var reservationId = 999; // Non-existent reservation

        _reservationRepositoryMock
            .Setup(r => r.GetByIdAsync(reservationId))
            .ReturnsAsync((Reservation?)null);

        // Act
        var result = await _reservationService.CancelAsync(reservationId);

        // Assert
        Assert.That(result, Is.False);
        _reservationRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Reservation>()), Times.Never);
        _roomRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Room>()), Times.Never);
    }

    [Test]
    public async Task GetByIdAsync_WhenReservationExists_ReturnsReservationDto()
    {
        // Arrange
        var reservationId = 1;
        var guest = new Guest
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            Phone = "0987654321"
        };

        var room = new Room
        {
            Id = 2,
            RoomNumber = "202",
            RoomTypeId = 1,
            Rate = 150.00m,
            IsAvailable = false
        };

        var reservation = new Reservation
        {
            Id = reservationId,
            GuestId = guest.Id,
            RoomId = room.Id,
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(4),
            Status = ReservationStatus.Confirmed,
            CreatedAt = DateTime.UtcNow,
            Guest = guest,
            Room = room
        };

        _reservationRepositoryMock
            .Setup(r => r.GetByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        // Act
        var result = await _reservationService.GetByIdAsync(reservationId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(reservationId));
        Assert.That(result.GuestId, Is.EqualTo(guest.Id));
        Assert.That(result.GuestName, Is.EqualTo("Jane Smith"));
        Assert.That(result.RoomId, Is.EqualTo(room.Id));
        Assert.That(result.RoomNumber, Is.EqualTo("202"));
        Assert.That(result.Status, Is.EqualTo(ReservationStatus.Confirmed));
    }

    [Test]
    public async Task GetByIdAsync_WhenReservationDoesNotExist_ReturnsNull()
    {
        // Arrange
        var reservationId = 999; // Non-existent reservation

        _reservationRepositoryMock
            .Setup(r => r.GetByIdAsync(reservationId))
            .ReturnsAsync((Reservation?)null);

        // Act
        var result = await _reservationService.GetByIdAsync(reservationId);

        // Assert
        Assert.That(result, Is.Null);
    }
}
