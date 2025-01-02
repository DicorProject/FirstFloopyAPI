using Floppy.Application.DTOs.Response;
using Floppy.Application.Models.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface ISellerInfoService
    {
        Task<ApiResponse<VendorRegistration>> GetVendorRegistrationDetailsData(int Id);
        Task<ApiResponse<ItemWithVendorDetailsResponse>> GetItemListByVendorId(int vendorId, double latiude, double longitude, int ItemId);

	}
}
