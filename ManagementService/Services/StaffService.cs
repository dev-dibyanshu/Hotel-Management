using ManagementService.DTOs;
using ManagementService.Models;
using ManagementService.Repositories;

namespace ManagementService.Services;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public StaffService(IStaffRepository staffRepository, IDepartmentRepository departmentRepository)
    {
        _staffRepository = staffRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<StaffDto>> GetAllAsync()
    {
        var staffList = await _staffRepository.GetAllAsync();
        return staffList.Select(s => new StaffDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            Phone = s.Phone,
            DepartmentId = s.DepartmentId,
            DepartmentName = s.Department?.Name ?? string.Empty,
            Role = s.Role,
            Salary = s.Salary,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt
        });
    }

    public async Task<StaffDto?> GetByIdAsync(int id)
    {
        var staff = await _staffRepository.GetByIdAsync(id);
        if (staff == null)
        {
            return null;
        }

        return new StaffDto
        {
            Id = staff.Id,
            FirstName = staff.FirstName,
            LastName = staff.LastName,
            Email = staff.Email,
            Phone = staff.Phone,
            DepartmentId = staff.DepartmentId,
            DepartmentName = staff.Department?.Name ?? string.Empty,
            Role = staff.Role,
            Salary = staff.Salary,
            IsActive = staff.IsActive,
            CreatedAt = staff.CreatedAt
        };
    }

    public async Task<StaffDto?> CreateAsync(CreateStaffDto createStaffDto)
    {
        // Check if department exists
        var department = await _departmentRepository.GetByIdAsync(createStaffDto.DepartmentId);
        if (department == null)
        {
            return null;
        }

        var staff = new Staff
        {
            FirstName = createStaffDto.FirstName,
            LastName = createStaffDto.LastName,
            Email = createStaffDto.Email,
            Phone = createStaffDto.Phone,
            DepartmentId = createStaffDto.DepartmentId,
            Role = createStaffDto.Role,
            Salary = createStaffDto.Salary,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdStaff = await _staffRepository.CreateAsync(staff);
        var loadedStaff = await _staffRepository.GetByIdAsync(createdStaff.Id);

        return new StaffDto
        {
            Id = loadedStaff!.Id,
            FirstName = loadedStaff.FirstName,
            LastName = loadedStaff.LastName,
            Email = loadedStaff.Email,
            Phone = loadedStaff.Phone,
            DepartmentId = loadedStaff.DepartmentId,
            DepartmentName = loadedStaff.Department?.Name ?? string.Empty,
            Role = loadedStaff.Role,
            Salary = loadedStaff.Salary,
            IsActive = loadedStaff.IsActive,
            CreatedAt = loadedStaff.CreatedAt
        };
    }

    public async Task<StaffDto?> UpdateAsync(int id, CreateStaffDto createStaffDto)
    {
        var staff = await _staffRepository.GetByIdAsync(id);
        if (staff == null)
        {
            return null;
        }

        // Check if department exists
        var department = await _departmentRepository.GetByIdAsync(createStaffDto.DepartmentId);
        if (department == null)
        {
            return null;
        }

        staff.FirstName = createStaffDto.FirstName;
        staff.LastName = createStaffDto.LastName;
        staff.Email = createStaffDto.Email;
        staff.Phone = createStaffDto.Phone;
        staff.DepartmentId = createStaffDto.DepartmentId;
        staff.Role = createStaffDto.Role;
        staff.Salary = createStaffDto.Salary;

        await _staffRepository.UpdateAsync(staff);
        var updatedStaff = await _staffRepository.GetByIdAsync(id);

        return new StaffDto
        {
            Id = updatedStaff!.Id,
            FirstName = updatedStaff.FirstName,
            LastName = updatedStaff.LastName,
            Email = updatedStaff.Email,
            Phone = updatedStaff.Phone,
            DepartmentId = updatedStaff.DepartmentId,
            DepartmentName = updatedStaff.Department?.Name ?? string.Empty,
            Role = updatedStaff.Role,
            Salary = updatedStaff.Salary,
            IsActive = updatedStaff.IsActive,
            CreatedAt = updatedStaff.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _staffRepository.DeleteAsync(id);
    }
}
