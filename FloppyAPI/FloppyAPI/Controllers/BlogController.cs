using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #region BlogList
        [HttpGet("BlogList")]
        public async Task<IActionResult> FetchBlogList(int startIndex, int PageSize)
        {
            var response = await _blogService.FetchBlogList(startIndex, PageSize);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region BlogDetailsGetById
        [HttpGet("blog_details_by_id/{PageId}")]
        public async Task<IActionResult> FetchBlogDetailsById(int PageId)
        {
            var response = await _blogService.GetBlogDetailsByPageId(PageId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region SaveBlog Review
        [HttpPost("saveBlogReview")]
        public async Task<IActionResult> SaveBlogReview([FromBody] BlogReviewInputRequest  request)
        {
            var response = await _blogService.saveBlogReview(request);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        #endregion

        #region GetReviewByBlogId
        [HttpGet("GetReviewListByBlogId/{BlogId}")]
        public async Task<IActionResult> GetReviewListByBlogId(int BlogId)
        {
            var response = await _blogService.GetReviewListByBlogId(BlogId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
