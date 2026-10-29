using System.ComponentModel.DataAnnotations;
using ManagementService.Models;

namespace ManagementService.DTOs;

public class CreatePaymentDto
{
    [Required]
    public int BillId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }
}
