using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        #region GetRatingByItemId
        [HttpGet("GetRatingByItemId/{id}")]
        public async Task<IActionResult> GetRatingByItemId(int id)
        {
            var response = await _ratingService.GetRatingByItemId(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        #endregion

        #region Insert Rating Review
        [HttpPost("insert-rating-review")]
        public async Task<IActionResult> InsertRatingReview([FromBody] RatingReviewDto ratingReview)
        {
            var response = await _ratingService.InsertRatingReviewAsync(ratingReview);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        #endregion
    }
}
