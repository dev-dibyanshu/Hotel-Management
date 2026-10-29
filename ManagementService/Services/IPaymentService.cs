using ManagementService.DTOs;

namespace ManagementService.Services;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync();
    Task<PaymentDto?> GetByIdAsync(int id);
    Task<PaymentDto?> CreateAsync(CreatePaymentDto createPaymentDto);
}
