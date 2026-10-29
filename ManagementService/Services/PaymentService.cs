using ManagementService.DTOs;
using ManagementService.Models;
using ManagementService.Repositories;

namespace ManagementService.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBillRepository _billRepository;

    public PaymentService(IPaymentRepository paymentRepository, IBillRepository billRepository)
    {
        _paymentRepository = paymentRepository;
        _billRepository = billRepository;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();
        return payments.Select(p => new PaymentDto
        {
            Id = p.Id,
            BillId = p.BillId,
            Amount = p.Amount,
            PaymentDate = p.PaymentDate,
            PaymentMethod = p.PaymentMethod,
            Status = p.Status
        });
    }

    public async Task<PaymentDto?> GetByIdAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
        {
            return null;
        }

        return new PaymentDto
        {
            Id = payment.Id,
            BillId = payment.BillId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status
        };
    }

    public async Task<PaymentDto?> CreateAsync(CreatePaymentDto createPaymentDto)
    {
        // Check if bill exists
        var bill = await _billRepository.GetByIdAsync(createPaymentDto.BillId);
        if (bill == null)
        {
            return null;
        }

        // Create payment
        var payment = new Payment
        {
            BillId = createPaymentDto.BillId,
            Amount = createPaymentDto.Amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = createPaymentDto.PaymentMethod,
            Status = "Completed"
        };

        var createdPayment = await _paymentRepository.CreateAsync(payment);

        // Update bill status to Paid if payment covers the bill amount
        if (createPaymentDto.Amount >= bill.Amount)
        {
            bill.Status = BillStatus.Paid;
            await _billRepository.UpdateAsync(bill);
        }

        return new PaymentDto
        {
            Id = createdPayment.Id,
            BillId = createdPayment.BillId,
            Amount = createdPayment.Amount,
            PaymentDate = createdPayment.PaymentDate,
            PaymentMethod = createdPayment.PaymentMethod,
            Status = createdPayment.Status
        };
    }
}
