using Floppy.Application.DTOs.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IPlaceEnquiryService
    {
        Task<ApiResponse<bool>> PlaceEnquiryForItem(PlaceEnquiryRequest request);
    }
}
