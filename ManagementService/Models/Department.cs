namespace ManagementService.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
