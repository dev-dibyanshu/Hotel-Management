using System.ComponentModel.DataAnnotations;

namespace ManagementService.DTOs;

public class CreateStaffDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
}
