using Sfarma.Api.DTOs;
using Sfarma.Api.Models;

namespace Sfarma.Api.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllAsync();
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<InvoiceDto> CreateAsync(InvoiceCreateDto dto);
    Task<bool> UpdateStateAsync(int id, InvoiceState state);
    Task<InvoiceDto?> CreateFromSaleOrderAsync(int saleOrderId);
    Task<InvoiceDto?> CreateFromPurchaseOrderAsync(int purchaseOrderId);
}
