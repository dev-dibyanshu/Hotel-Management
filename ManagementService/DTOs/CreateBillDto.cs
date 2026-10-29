using System.ComponentModel.DataAnnotations;

namespace ManagementService.DTOs;

public class CreateBillDto
{
    [Required]
    public int ReservationId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
}
