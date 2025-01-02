using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class HomeService :IHomeService
    {
        private readonly IHomeRepository _homeRepository;
        private readonly ILogger<HomeService> _logger;
        public HomeService(IHomeRepository homeRepository, ILogger<HomeService> logger)
        {

            _homeRepository = homeRepository;
            _logger = logger;   

        }
        #region Fetch All Dynamic Data for Home Page
        public async Task<ApiResponse<Dictionary<string, List<Dictionary<string, object>>>>> FetchAllDynamicDataForHomePage()
        {
            var response = new ApiResponse<Dictionary<string, List<Dictionary<string, object>>>>();

            try
            {
                // Fetch all dynamic data for the home page from the repository
                var homePageSliders = await _homeRepository.GetAllDynamicDataForHomePageAsync();

                if (homePageSliders != null && homePageSliders.Any())
                {
                    response.Success = true;
                    response.Message = "Home page sliders retrieved successfully";
                    response.Data = homePageSliders;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No sliders found";
                    response.Data = new Dictionary<string, List<Dictionary<string, object>>>(); 
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

        #region Fetch All List of Items for HomePage 
        public async Task<ApiResponse<List<Item>>> GetItemList()
        {
            var response = new ApiResponse<List<Item>>();

            try
            {
                // Fetch all dynamic data for the home page from the repository
                var itemList = await _homeRepository.GetAllItemList();

                if (itemList != null && itemList.Any())
                {
                    response.Success = true;
                    response.Message = "Item list retrieved successfully.";
                    response.Data = itemList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No items found.";
                    response.Data = new List<Item>();
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

        #region Get list of category with subcategorylist
        public async Task<ApiResponse<List<CategoryDto>>> GetCategorywithSubCategoryList()
        {
            var response = new ApiResponse<List<CategoryDto>>();

            try
            {
                var itemList = await _homeRepository.GetCategoriesWithSubcategoriesAsync();

                if (itemList != null && itemList.Any())
                {
                    response.Success = true;
                    response.Message = "Category with SubCategory list retrieved successfully.";
                    response.Data = itemList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Category with SubCategory list.";
                    response.Data = new List<CategoryDto>();
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

        #region AreaList
        public async Task<ApiResponse<List<CityModel>>> GetAreasList()
        {
            var response = new ApiResponse<List<CityModel>>();

            try
            {
                var areaList = await _homeRepository.GetVendorRegistrationList();

                if (areaList != null && areaList.Any())
                {
                    response.Success = true;
                    response.Message = "Area list retrieved successfully.";
                    response.Data = areaList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No areas found.";
                    response.Data = new List<CityModel>();
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
