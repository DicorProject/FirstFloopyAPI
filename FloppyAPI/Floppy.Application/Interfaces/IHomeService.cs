using Floppy.Application.DTOs.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IHomeService
    {
        Task<ApiResponse<Dictionary<string, List<Dictionary<string, object>>>>> FetchAllDynamicDataForHomePage();
        Task<ApiResponse<List<Item>>> GetItemList();
        Task<ApiResponse<List<CategoryDto>>> GetCategorywithSubCategoryList();
        Task<ApiResponse<List<CityModel>>> GetAreasList();
    }
}
