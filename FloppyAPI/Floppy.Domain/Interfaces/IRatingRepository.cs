using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IRatingRepository
    {
        Task<List<RatingReview>> GetRatingByIdAsync(int Id);
        Task<bool> InsertRatingReviewAsync(RatingReviewDto ratingReview);
        Task<List<RatingReview>> GetRatingsByItemIdsAsync(List<int?> itemIds);
        Task<List<RatingReviewWithUserInfo>> GetRatingByIdAsyncData(int id);
    }
}
