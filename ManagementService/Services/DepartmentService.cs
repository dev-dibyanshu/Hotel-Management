using ManagementService.DTOs;
using ManagementService.Models;
using ManagementService.Repositories;

namespace ManagementService.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(d => new DepartmentDto
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description
        });
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            return null;
        }

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description
        };
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = new Department
        {
            Name = createDepartmentDto.Name,
            Description = createDepartmentDto.Description
        };

        var createdDepartment = await _departmentRepository.CreateAsync(department);

        return new DepartmentDto
        {
            Id = createdDepartment.Id,
            Name = createdDepartment.Name,
            Description = createdDepartment.Description
        };
    }

    public async Task<DepartmentDto?> UpdateAsync(int id, CreateDepartmentDto createDepartmentDto)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            return null;
        }

        department.Name = createDepartmentDto.Name;
        department.Description = createDepartmentDto.Description;

        var updatedDepartment = await _departmentRepository.UpdateAsync(department);

        return new DepartmentDto
        {
            Id = updatedDepartment!.Id,
            Name = updatedDepartment.Name,
            Description = updatedDepartment.Description
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _departmentRepository.DeleteAsync(id);
    }
}
