using ManagementService.DTOs;

namespace ManagementService.Services;

public interface IBillService
{
    Task<IEnumerable<BillDto>> GetAllAsync();
    Task<BillDto?> GetByIdAsync(int id);
    Task<BillDto> CreateAsync(CreateBillDto createBillDto);
    Task<BillDto?> UpdateAsync(int id, CreateBillDto createBillDto);
    Task<bool> DeleteAsync(int id);
}
