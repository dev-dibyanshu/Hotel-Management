using ManagementService.DTOs;

namespace ManagementService.Services;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> GetAllAsync();
    Task<StaffDto?> GetByIdAsync(int id);
    Task<StaffDto?> CreateAsync(CreateStaffDto createStaffDto);
    Task<StaffDto?> UpdateAsync(int id, CreateStaffDto createStaffDto);
    Task<bool> DeleteAsync(int id);
}
