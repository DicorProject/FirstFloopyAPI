using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public ServiceController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #region ServicesList
        [HttpGet("ServicesList")]
        public async Task<IActionResult> Categories()
        {
            var response = await _categoryService.GetAllCategories();

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
		#endregion

		#region GetServicesListByCategoryandsubcategoryId
		[HttpGet("GetServicesListByIds/{categoryId}")]
		public async Task<IActionResult> ServicesListByCategoryandsubcategoryId(int categoryId)
		{
			var response = await _categoryService.GetAllServices(categoryId);

			if (response.Success)
			{
                return Ok(response);
			}
			return BadRequest(response);
		}
		#endregion

		#region ItemListWithDetailsByCategoryIdandSubCategoryId
		[HttpPost("GetItemsByCategory")]
        public async Task<IActionResult> ItemListWithDetailsByCategoryIdandSubCategoryId([FromBody] ItemRequestModel request)
        {
            var response = await _categoryService.GetAllItemByCategoryIdAndSubId(
                  request.SubCategoryIds,request.ServicesName ,request.CategoryId, request.Latitude, request.Longitude, request.StartIndex, request.PageSize);

            if (response != null && response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        #endregion

        #region ServiceDetails
        [HttpGet("ServiceDeatailsById/{ServiceId}")]
        public async Task<IActionResult> ServiceDetailsById(int ServiceId)
        {
            var response = await _categoryService.GetServiceDetailsByIdAsync(ServiceId);

            if (response != null && response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        #endregion

        #region Ratepage
        [HttpGet("service_page")]
        public async Task<IActionResult> ServicePageList(int CategoryId)
        {
            var response = await _categoryService.GetServicepageList(CategoryId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region SearchItem
        [HttpPost("searchItems")]
        public async Task<IActionResult> SearchItems([FromBody] SearchItemRequestModel request)
        {
            var response = await _categoryService.SearchItemAsync(request.subgroupIds,request.serviceNames, request.Location, request.Latitude, request.Longitude);

            if (response != null && response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        #endregion

        #region GetTax
        [HttpGet("taxandfees")]
        public async Task<IActionResult> GetTaxandFees()
        {
            var response = await _categoryService.GetTax();

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
