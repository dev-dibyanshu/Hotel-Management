using ManagementService.Models;

namespace ManagementService.Repositories;

public interface IStaffRepository
{
    Task<IEnumerable<Staff>> GetAllAsync();
    Task<Staff?> GetByIdAsync(int id);
    Task<Staff> CreateAsync(Staff staff);
    Task<Staff?> UpdateAsync(Staff staff);
    Task<bool> DeleteAsync(int id);
}
