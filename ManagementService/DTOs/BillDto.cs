using ManagementService.Models;

namespace ManagementService.DTOs;

public class BillDto
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public BillStatus Status { get; set; }
}
