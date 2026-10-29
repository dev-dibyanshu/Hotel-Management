using ManagementService.Models;

namespace ManagementService.Repositories;

public interface IBillRepository
{
    Task<IEnumerable<Bill>> GetAllAsync();
    Task<Bill?> GetByIdAsync(int id);
    Task<Bill> CreateAsync(Bill bill);
    Task<Bill?> UpdateAsync(Bill bill);
    Task<bool> DeleteAsync(int id);
}
