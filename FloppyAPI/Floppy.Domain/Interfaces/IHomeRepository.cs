using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IHomeRepository
    {
        Task<Dictionary<string, List<Dictionary<string, object>>>> GetAllDynamicDataForHomePageAsync();
        Task<List<Item>> GetAllItemList();
        Task<List<CategoryDto>> GetCategoriesWithSubcategoriesAsync();
        Task<List<string>> GetAreasList();
        Task<List<CityModel>> GetVendorRegistrationList();
    }
}
