using Microsoft.EntityFrameworkCore;
using HotelService.Models;

namespace HotelService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Guest> Guests { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Guest configuration
        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // RoomType configuration
        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Capacity).IsRequired();
        });

        // Room configuration
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RoomNumber).IsUnique();
            entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Rate).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.IsAvailable).IsRequired().HasDefaultValue(true);

            entity.HasOne(e => e.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(e => e.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Reservation configuration
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CheckInDate).IsRequired();
            entity.Property(e => e.CheckOutDate).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>().IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(e => e.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
