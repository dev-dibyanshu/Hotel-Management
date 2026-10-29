using System.ComponentModel.DataAnnotations;

namespace ManagementService.DTOs;

public class UpdateInventoryQuantityDto
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }
}
