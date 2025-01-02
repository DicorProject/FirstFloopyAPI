using Floppy.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        #region FetchAllDynamicDataforHomePage
        [HttpGet("fetch-all-dynamic-data")]
        public async Task<IActionResult> FetchAllDynamicDataForHomePage()
        {
            var response = await _homeService.FetchAllDynamicDataForHomePage();

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        #endregion

        #region GetItemList
        [HttpGet("item-list")]
        public async Task<IActionResult> GetItemList()
        {
            var response = await _homeService.GetItemList();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region GetCategoryWithSubCategoryList
        [HttpGet("Category-with-subcategory-list")]
        public async Task<IActionResult> GetCategorywithsubcategoryList()
        {
            var response = await _homeService.GetCategorywithSubCategoryList();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
