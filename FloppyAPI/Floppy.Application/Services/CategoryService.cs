using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Response;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRatingRepository _ratingRepository;  
        private readonly ILogger<CategoryService> _logger;  
        public CategoryService(ICategoryRepository categoryRepository,IRatingRepository ratingRepository,ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _ratingRepository = ratingRepository;
            _logger = logger;   
        }
        #region ServicesList
        public async Task<ApiResponse<List<object>>> GetAllCategories()
        {
            var response = new ApiResponse<List<object>>();

            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();

                if (categories != null && categories.Any())
                {
                    response.Success = true;
                    response.Message = "Categories retrieved successfully";
                    response.Data = categories;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No categories found";
                    response.Data = new List<object>();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = null;
            }

            return response;
        }

        #endregion

        #region subcategoriesList
        public async Task<ApiResponse<List<Dictionary<string, object>>>> GetAllSubCategoryByCategoryId(int categoryId)
        {
            var response = new ApiResponse<List<Dictionary<string, object>>>();

            try
            {
                var subcategoriesData = await _categoryRepository.GetAllSubCategoriesByCategoryIdAsync(categoryId);

                if (subcategoriesData != null && subcategoriesData.Any())
                {
                    response.Success = true;
                    response.Message = "Subcategories retrieved successfully";
                    response.Data = subcategoriesData;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No subcategories found for the given category ID";
                    response.Data = new List<Dictionary<string, object>>();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion

        #region ItemListWithVendorDetails
        public async Task<ApiResponse<ItemWithVendorDetailsResponse>> GetAllItemByCategoryIdAndSubId(List<int> subIds, List<string> serviceNames, int catId, double latiude, double longitude, int startIndex, int pageSize)
        {
            var response = new ApiResponse<ItemWithVendorDetailsResponse>();

            try
            {
                var (items, totalCount) = await _categoryRepository.GetAllItemListWithDetails(subIds, serviceNames, catId, startIndex, pageSize);

                if (items != null && items.Any())
                {
                    var vendorIds = items.Select(item => item.vendorid).Distinct().ToList();
                    var vendors = await _categoryRepository.GetNearbyVendorsByIdsAndLocation(vendorIds, latiude, longitude);
                    var itemIds = items.Select(item => item.id).Distinct().ToList();
                    var reviews = await _ratingRepository.GetRatingsByItemIdsAsync(itemIds);
					var itemsWithVendorDetails = items
						   .Select(item => new ItemWithVendorDetails
						   {
							   Item = item,
							   Reviews = reviews.Where(r => r.ItemId == item.id).ToList(),
							   Vendor = vendors.FirstOrDefault(v => v.Id == item.vendorid)
						   })
                             .Where(itemWithVendor => itemWithVendor.Vendor != null
        && (itemWithVendor.Item.maingroupid == 7 || itemWithVendor.Item.maingroupid == 8
            || itemWithVendor.Vendor.Distance <= 100))
    .OrderBy(itemWithVendor => itemWithVendor.Vendor.Distance) // Sorting in ascending order by Vendor.Distance
    .ToList();
                    response.Success = true;
                    response.Message = "Items and vendor details retrieved successfully";
                    response.Data = new ItemWithVendorDetailsResponse
                    {
                        Items = itemsWithVendorDetails,
                        TotalItems = itemsWithVendorDetails.Count
					}; 
                }
                else
                {
                    response.Success = false;
                    response.Message = "No items found for the given category ID";
                    response.Data = new ItemWithVendorDetailsResponse();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion

        #region ServiceDetailsById
        public async Task<ApiResponse<ServiceDetailsResponse>> GetServiceDetailsByIdAsync(int id)
        {
            var response = new ApiResponse<ServiceDetailsResponse>();

            try
            {
                // Retrieve service and rating details
                var service = await _categoryRepository.GetItemByIdAsync(id);
                var rating = await _ratingRepository.GetRatingByIdAsync(id);

                if (service != null)
                {
                    // Populate the response object
                    response.Success = true;
                    response.Message = "Service details successfully retrieved.";
                    response.Data = new ServiceDetailsResponse
                    {
                        item = service,
                        ratingReview = rating
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "No service found with the specified ID.";
                    response.Data = new ServiceDetailsResponse
                    {
                        item = null,
                        ratingReview = null
                    };
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving the service details: {ex.Message}";
                response.Data = new ServiceDetailsResponse
                {
                    item = null,
                    ratingReview = null
                };
            }

            return response;
        }

        #endregion

        #region RatePage
        public async Task<ApiResponse<List<CategoryData>>> GetServicepageList(int CategoryId)
        {
            var response = new ApiResponse<List<CategoryData>>();

            try
            {
                var leadEntries = await _categoryRepository.GetCategoriesWithSubCategoriesAndFirstPriceAsync(CategoryId);

                if (leadEntries != null && leadEntries.Any())
                {
                    response.Success = true;
                    response.Message = "Order details fetched successfully.";
                    response.Data = leadEntries;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No order details found for the given user.";
                    response.Data = new List<CategoryData>();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while fetching order details: {ex.Message}";
                response.Data = null;
            }
            return response;
        }
        #endregion

        #region SearchItem
        public async Task<ApiResponse<ItemWithVendorDetailsResponse>> SearchItemAsync(List<int?> subgroupIds, List<string> serviceNames, string location,double latitude,double longitude)
        {
            var response = new ApiResponse<ItemWithVendorDetailsResponse>();

            try
            {
                // Validate the input parameters
                if (subgroupIds == null || !subgroupIds.Any())
                {
                    response.Success = false;
                    response.Message = "Invalid subgroup ID.";
                    response.Data = new ItemWithVendorDetailsResponse();
                    return response;
                }

                if (string.IsNullOrWhiteSpace(location))
                {
                    response.Success = false;
                    response.Message = "Location cannot be null or empty.";
                    response.Data = new ItemWithVendorDetailsResponse();
                    return response;
                }
                var items = await _categoryRepository.GetItemsBySubGroupIdwithLatitudeandLongitudeAsync(subgroupIds,serviceNames,latitude,longitude);
                if (items != null && items.Any())
                {
                    var vendorIds = items.Select(item => item.vendorid).Distinct().ToList();
                    var vendors = await _categoryRepository.GetNearbyVendorsByIdsAndLocation(vendorIds, latitude, longitude);
                    var itemIds = items.Select(item => item.id).Distinct().ToList();
                    var reviews = await _ratingRepository.GetRatingsByItemIdsAsync(itemIds);
                    var SearchItemDetails = items.Select(item => new ItemWithVendorDetails
                    {
                        Item = item,
                        Reviews = reviews.Where(r => r.ItemId == item.id).ToList(),
                        Vendor = vendors.FirstOrDefault(v => v.Id == item.vendorid),
                    }).ToList();
                    response.Success = true;
                    response.Message = "Items Details retrieved successfully.";
                    response.Data = new ItemWithVendorDetailsResponse
                    {
                        Items = SearchItemDetails,
                        TotalItems = items.Count,
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "No items found for the given subgroup ID and location.";
                    response.Data = new ItemWithVendorDetailsResponse();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = null;
            }

            return response;
        }

        #endregion

        #region Tax
        public async Task<ApiResponse<double>> GetTax()
        {
            var response = new ApiResponse<double>();

            try
            {
                // Fetch tax fees from the repository
                var taxes = await _categoryRepository.GetTaxFeesAsync();
                if (taxes != null)
                {
                    response.Success = true;
                    response.Message = "Taxes retrieved successfully";
                    response.Data = taxes;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No taxes found";
                    response.Data = 0.0; 
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = 0.0; 
            }

            return response;
        }

		#endregion

		#region ServicesList
		public async Task<ApiResponse<List<Dictionary<string, object>>>> GetAllServices(int categoryId)
		{
			var response = new ApiResponse<List<Dictionary<string, object>>>();

			try
			{
				var categories = await _categoryRepository.GetAllServicesAsync(categoryId);

				if (categories != null && categories.Any())
				{
					response.Success = true;
					response.Message = "Services retrieved successfully";
					response.Data = categories;
				}
				else
				{
					response.Success = false;
					response.Message = "No services found";
					response.Data = new List<Dictionary<string, object>>();
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"An error occurred: {ex.Message}";
				response.Data = null;
			}

			return response;
		}

		#endregion
	}
}
