using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ILogger<RatingService> _logger;    
        public RatingService(IRatingRepository ratingRepository,ILogger<RatingService> logger)
        {
            _ratingRepository = ratingRepository;
            _logger = logger;   
        }
        #region GetRatingById
        public async Task<ApiResponse<List<RatingReviewWithUserInfo>>> GetRatingByItemId(int id)
        {
            var response = new ApiResponse<List<RatingReviewWithUserInfo>>();

            try
            {
                var ratingData = await _ratingRepository.GetRatingByIdAsyncData(id);

                if (ratingData != null)
                {
                    response.Success = true;
                    response.Message = "Rating data retrieved successfully.";
                    response.Data = ratingData;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Rating data not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving the rating data: {ex.Message}";
            }

            return response;
        }
        #endregion

        #region InsertRating
        public async Task<ApiResponse<bool>> InsertRatingReviewAsync(RatingReviewDto ratingReview)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var isInserted = await _ratingRepository.InsertRatingReviewAsync(ratingReview);

                if (isInserted)
                {
                    response.Success = true;
                    response.Message = "Rating review inserted successfully.";
                    response.Data = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to insert rating review.";
                    response.Data = false;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while inserting the rating review: {ex.Message}";
                response.Data = false;
            }

            return response;
        }
        #endregion
    }
}
