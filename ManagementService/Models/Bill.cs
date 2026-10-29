namespace ManagementService.Models;

public class Bill
{
    public int Id { get; set; }
    public int ReservationId { get; set; }  // ID from HotelService (not a foreign key)
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; } = DateTime.UtcNow;
    public BillStatus Status { get; set; } = BillStatus.Pending;

    // Navigation property
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
