using Floppy.Application.DTOs.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IRatingService
    {
        Task<ApiResponse<List<RatingReviewWithUserInfo>>> GetRatingByItemId(int id);
        Task<ApiResponse<bool>> InsertRatingReviewAsync(RatingReviewDto ratingReview);
    }
}
