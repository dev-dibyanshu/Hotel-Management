using ManagementService.Models;

namespace ManagementService.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public int BillId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Status { get; set; } = string.Empty;
}
