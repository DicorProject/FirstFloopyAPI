using cashfree_pg.Model;
using Floppy.Application.Models.Request;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IPaymentService
    {
        CashFreePaymentResponse CreateOrder(OrderRequest request);
        DTOs.Response.ApiResponse<string> UpdatepaymentDetailsByUserId(paymentUpdate request);
        DTOs.Response.ApiResponse<bool> PlaceEnquiryForItem(PlaceEnquiryRequest request);
    }
}
