namespace Sfarma.Api.DTOs;

public record DashboardSummaryDto(
    int Productos,
    int Partners,
    int SaleOrders,
    decimal SaleOrdersTotal,
    int PurchaseOrders,
    decimal PurchaseOrdersTotal,
    int Invoices,
    decimal InvoicesTotal
);
