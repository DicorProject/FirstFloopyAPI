using Floppy.Application.DTOs.Response;
using Floppy.Application.Models.Request;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<string>> SaveOrderDetails(OrderRequest request);
        Task<ApiResponse<List<LeadWithCartItemsDTO>>> FetchorderDetails(int userId);
        Task<ApiResponse<LeadByProductModel>> ReviewOrderDetails(int orderId);
        Task<ApiResponse<bool>> AddCartDetails(List<CartItemDTO> cartMasters);
        Task<ApiResponse<bool>> UpdateCartDetails(List<CartItemDTO> cartMasters);
        Task<ApiResponse<List<ResponseItemDTO>>> GetCartDetailsAsync(int userId);
        Task<ApiResponse<bool>> DeleteCartAsync(List<int> ids);
        Task<ApiResponse<bool>> CancelOrderAsync(int id);
        Task<ApiResponse<bool>> UpdateOrderAsync(int id, string newSlot, DateTime newDateTimestamp, int UserId);
    }
}
