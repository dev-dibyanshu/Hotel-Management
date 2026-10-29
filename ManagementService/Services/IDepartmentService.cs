using ManagementService.DTOs;

namespace ManagementService.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDepartmentDto);
    Task<DepartmentDto?> UpdateAsync(int id, CreateDepartmentDto createDepartmentDto);
    Task<bool> DeleteAsync(int id);
}
