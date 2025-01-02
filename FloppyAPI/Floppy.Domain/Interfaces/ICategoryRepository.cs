using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<object>> GetAllCategoriesAsync();
        Task<List<Dictionary<string, object>>> GetAllSubCategoriesByCategoryIdAsync(int categoryId);
        Task<(List<Item> Items, int TotalCount)> GetAllItemListWithDetails(List<int> subids, List<string> serviceNames, int mainid, int startIndex = 0, int pageSize = 0);
        Task<ItemDeatsils> GetItemByIdAsync(int id);
        Task<List<CategoryData>> GetCategoriesWithSubCategoriesAndFirstPriceAsync(int CategoryId);
        Task<List<Item>> GetItemsBySubGroupIdAsync(int subgroupId, string location);
        Task<List<VendorRegistration>> GetVendorDetailsByIds(List<int?> vendorIds);
        Task<List<VendorDetails>> GetNearbyVendorsByIdsAndLocation(List<int?> vendorIds, double currentLatitude, double currentLongitude);
        Task<double> GetTaxFeesAsync();
        Task<List<Item>> GetItemsBySubGroupIdwithLatitudeandLongitudeAsync(List<int?> subgroupIds, List<string> serviceNames, double latitude, double longitude);
        Task<List<Dictionary<string, object>>> GetAllServicesAsync(int categoryId);
        Task<List<Item>> GetItemListWithDetailsByVendorIdAsync(int vendorId);

	}
}
