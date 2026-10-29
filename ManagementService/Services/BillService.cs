using ManagementService.DTOs;
using ManagementService.Models;
using ManagementService.Repositories;

namespace ManagementService.Services;

public class BillService : IBillService
{
    private readonly IBillRepository _billRepository;

    public BillService(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<IEnumerable<BillDto>> GetAllAsync()
    {
        var bills = await _billRepository.GetAllAsync();
        return bills.Select(b => new BillDto
        {
            Id = b.Id,
            ReservationId = b.ReservationId,
            Amount = b.Amount,
            BillDate = b.BillDate,
            Status = b.Status
        });
    }

    public async Task<BillDto?> GetByIdAsync(int id)
    {
        var bill = await _billRepository.GetByIdAsync(id);
        if (bill == null)
        {
            return null;
        }

        return new BillDto
        {
            Id = bill.Id,
            ReservationId = bill.ReservationId,
            Amount = bill.Amount,
            BillDate = bill.BillDate,
            Status = bill.Status
        };
    }

    public async Task<BillDto> CreateAsync(CreateBillDto createBillDto)
    {
        var bill = new Bill
        {
            ReservationId = createBillDto.ReservationId,
            Amount = createBillDto.Amount,
            BillDate = DateTime.UtcNow,
            Status = BillStatus.Pending
        };

        var createdBill = await _billRepository.CreateAsync(bill);

        return new BillDto
        {
            Id = createdBill.Id,
            ReservationId = createdBill.ReservationId,
            Amount = createdBill.Amount,
            BillDate = createdBill.BillDate,
            Status = createdBill.Status
        };
    }

    public async Task<BillDto?> UpdateAsync(int id, CreateBillDto createBillDto)
    {
        var bill = await _billRepository.GetByIdAsync(id);
        if (bill == null)
        {
            return null;
        }

        bill.ReservationId = createBillDto.ReservationId;
        bill.Amount = createBillDto.Amount;

        var updatedBill = await _billRepository.UpdateAsync(bill);

        return new BillDto
        {
            Id = updatedBill!.Id,
            ReservationId = updatedBill.ReservationId,
            Amount = updatedBill.Amount,
            BillDate = updatedBill.BillDate,
            Status = updatedBill.Status
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _billRepository.DeleteAsync(id);
    }
}
