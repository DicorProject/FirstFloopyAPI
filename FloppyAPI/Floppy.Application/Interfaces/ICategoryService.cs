using Floppy.Application.DTOs.Response;
using Floppy.Application.Models.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<object>>> GetAllCategories();
        Task<ApiResponse<List<Dictionary<string, object>>>> GetAllSubCategoryByCategoryId(int categoryId);
        Task<ApiResponse<ItemWithVendorDetailsResponse>> GetAllItemByCategoryIdAndSubId(List<int> subIds, List<string> serviceNames, int catId, double latiude, double longitude, int startIndex, int pageSize);
        Task<ApiResponse<ServiceDetailsResponse>> GetServiceDetailsByIdAsync(int id);
        Task<ApiResponse<List<CategoryData>>> GetServicepageList(int CategoryId);
        Task<ApiResponse<ItemWithVendorDetailsResponse>> SearchItemAsync(List<int?> subgroupIds,List<string> serviceNames, string location, double latitude, double longitude);
        Task<ApiResponse<double>> GetTax();
        Task<ApiResponse<List<Dictionary<string, object>>>> GetAllServices(int categoryId);

	}
}
