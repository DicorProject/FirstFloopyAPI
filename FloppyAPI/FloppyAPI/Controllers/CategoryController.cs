using Floppy.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #region CategoryList
        [HttpGet("CategoryList")]
        public async Task<IActionResult> GetServicesByCategoryId()
        {
            var response = await _categoryService.GetAllCategories();

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region subCategories
        [HttpGet("SubCategories/{categoryId}")]
        public async Task<IActionResult> GetSubCategoriesByCategoryId(int categoryId)
        {
            var response = await _categoryService.GetAllSubCategoryByCategoryId(categoryId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion


    }
}
