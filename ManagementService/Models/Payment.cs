namespace ManagementService.Models;

public class Payment
{
    public int Id { get; set; }
    public int BillId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMethod PaymentMethod { get; set; }
    public string Status { get; set; } = "Completed";

    // Navigation property
    public Bill Bill { get; set; } = null!;
}
