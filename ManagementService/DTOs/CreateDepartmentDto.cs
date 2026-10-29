using System.ComponentModel.DataAnnotations;

namespace ManagementService.DTOs;

public class CreateDepartmentDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
