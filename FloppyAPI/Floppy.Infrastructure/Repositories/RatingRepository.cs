using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RatingRepository> _logger;
        public RatingRepository(ApplicationDbContext context,ILogger<RatingRepository> logger)
        {
            _context = context; 
            _logger = logger;
        }
		#region GetUserRatingById
		public async Task<List<RatingReviewWithUserInfo>> GetRatingByIdAsyncData(int id)
        {
            // Query to fetch RatingReview data
            var ratingReviewSql = "SELECT * FROM rating_review WHERE itemid = @Id";
            var ratingReviewParameters = new[] { new SqlParameter("@Id", id) };

            var ratingReviews = await _context.rating_review
                .FromSqlRaw(ratingReviewSql, ratingReviewParameters)
                .ToListAsync();

            if (ratingReviews == null || !ratingReviews.Any())
            {
                return null;
            }

            var result = new List<RatingReviewWithUserInfo>();

            foreach (var ratingReview in ratingReviews)
            {
                Auth authUser = null;

                if (ratingReview.UserId == null)
                {
                    // Query to fetch Auth data based on Name
                    var authByNameSql = "SELECT * FROM tb_login WHERE name = @Name";
                    var authByNameParameters = new[] { new SqlParameter("@Name", ratingReview.Name ?? (object)DBNull.Value) };

                    authUser = await _context.tb_login
                        .FromSqlRaw(authByNameSql, authByNameParameters)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    // Query to fetch Auth data based on UserId
                    var authSql = "SELECT * FROM tb_login WHERE UserId = @UserId";
                    var authParameters = new[] { new SqlParameter("@UserId", ratingReview.UserId) };

                    authUser = await _context.tb_login
                        .FromSqlRaw(authSql, authParameters)
                        .FirstOrDefaultAsync();
                }

                result.Add(new RatingReviewWithUserInfo
                {
                    Id = ratingReview.Id,
                    PartyId = ratingReview.PartyId,
                    EntryDate = ratingReview.EntryDate,
                    ItemId = ratingReview.ItemId,
                    EntryType = ratingReview.EntryType,
                    Rating = ratingReview.Rating,
                    Review = ratingReview.Review,
                    Type = ratingReview.Type,
                    ApproveStatus = ratingReview.ApproveStatus,
                    Name = ratingReview.Name,
                    RatingValue = ratingReview.RatingValue,
                    Email = ratingReview.Email,
                    Phone = ratingReview.Phone,
                    UserId = ratingReview.UserId,
                    Address = authUser?.Address,
                    Locality = authUser?.Locality,
                    City = authUser?.City,
                    State = authUser?.State,
                    Pincode = authUser?.Pincode,
                    Image = authUser?.Image
                });
            }

            return result;
        }
		#endregion

		#region GetRatingById
		public async Task<List<RatingReview>> GetRatingByIdAsync(int Id)
        {
            var sql = @" SELECT * FROM rating_review WHERE itemid = @Id";

            var parameters = new[]
            {
                new SqlParameter("@Id", Id)
            };
            return await _context.rating_review
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }
		#endregion

		#region InsertRatingReview
		public async Task<bool> InsertRatingReviewAsync(RatingReviewDto ratingReview)
        {
            var sql = @"INSERT INTO rating_review 
                        ([entrydate], [itemid], [rating], [review], [type], [Name], [ratingvalue],[email],[phone],[userid])
                         VALUES 
                         (@EntryDate, @ItemId, @Rating, @Review, @Type, @Name, @RatingValue,@Email,@phone,@UserId)";

            var parameters = new[]
            {
                new SqlParameter("@EntryDate", DateTime.Now),
                new SqlParameter("@ItemId", ratingReview.ItemId ?? (object)DBNull.Value),
                new SqlParameter("@Rating", ratingReview.Rating ?? (object)DBNull.Value),
                new SqlParameter("@Review", ratingReview.Review ?? (object)DBNull.Value),
                new SqlParameter("@Type", ratingReview.Type ?? (object)DBNull.Value),
                new SqlParameter("@Name", ratingReview.Name ?? (object)DBNull.Value),
                new SqlParameter("@RatingValue", ratingReview.RatingValue ?? (object)DBNull.Value),
                new SqlParameter("@Email",ratingReview.Email ?? (object)DBNull.Value),  
                new SqlParameter("@phone",ratingReview.phone ?? (object)DBNull.Value),
                new SqlParameter("@UserId",ratingReview.UserId ?? (object)DBNull.Value),
            };
            var result = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            return result > 0;
        }
		#endregion

		#region FetchRatingsByItemIds
		public async Task<List<RatingReview>> GetRatingsByItemIdsAsync(List<int?> itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return new List<RatingReview>();

            var sql = @"
                    SELECT * FROM rating_review
                    WHERE itemid IN (" + string.Join(", ", itemIds.Select((id, index) => $"@id{index}")) + ")";

            var parameters = itemIds.Select((id, index) => new SqlParameter($"@id{index}", id)).ToArray();

            return await _context.rating_review
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }
		#endregion

	}
}
