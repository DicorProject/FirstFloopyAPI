using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlAddressRepositary _urlAddress;
        private readonly ILogger<BlogRepository> _logger;   
        public BlogRepository(ApplicationDbContext context, IUrlAddressRepositary urlAddress,ILogger<BlogRepository> logger)
        {
            _context = context;
            _urlAddress = urlAddress;
            _logger = logger;
        }
        #region GetBlogList
        public async Task<(List<BlogMaster> MetaTags, int TotalCount)> GetBlogListAsync(int startIndex, int pageSize)
        {
            var baseUrl = await _urlAddress.GetBaseUrlAsync();
            IQueryable<BlogMaster> query = _context.BlogMaster
                .Where(b => b.Status == 1)  // Ensure filtering by Status = 1
                .AsNoTracking();

            // Fetch category names and group by MainId
            var categoryQuery = _context.ClassificationMaster
                .AsNoTracking()
                .GroupBy(c => c.MainId)
                .Select(g => new
                {
                    MainId = g.Key,
                    CategoryName = g.Select(c => c.ClaasificationName).FirstOrDefault()  // Get the first category name
                });

            if (startIndex == 0 && pageSize == 0)
            {
                var allMetaTags = await query
                    .OrderBy(mt => mt.Id)
                    .ToListAsync();

                // Fetch category names for each MainId
                var categoryData = await categoryQuery.ToListAsync();
                var transformedMetaTags = allMetaTags
                    .Select(blog => new BlogMaster
                    {
                        Id = blog.Id,
                        MainId = blog.MainId,
                        SubId = blog.SubId,
                        BlogDescription = blog.BlogDescription,
                        Image = string.IsNullOrEmpty(blog.Image) ? null : $"{baseUrl}{blog.Image}",
                        Status = blog.Status,
                        CompId = blog.CompId,
                        BranchId = blog.BranchId,
                        Userid = blog.Userid,
                        YearId = blog.YearId,
                        Tittle = blog.Tittle,
                        Author = blog.Author,
                        // Fetch the first category name for the MainId
                        CategoryName = categoryData
                            .Where(c => c.MainId == blog.MainId)
                            .Select(c => c.CategoryName)
                            .FirstOrDefault()
                    })
                    .ToList();

                int totalCount = allMetaTags.Count;
                return (transformedMetaTags, totalCount);
            }
            else
            {
                // Fetch the paginated blog data
                var pagedMetaTags = await _context.BlogMaster
                    .Where(b => b.Status == 1)
                    .OrderBy(b => b.Id)
                    .Skip(startIndex)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();

                // Fetch the category data and group by MainId
                var categoryData = await _context.ClassificationMaster
                    .AsNoTracking()
                    .GroupBy(c => c.MainId)
                    .Select(g => new
                    {
                        MainId = g.Key,
                        CategoryName = g.Select(c => c.ClaasificationName).FirstOrDefault() // Get the first category name
                    })
                    .ToListAsync();

                // Transform the paginated blog data by adding the category names
                var transformedPagedMetaTags = pagedMetaTags
                    .Select(blog => new BlogMaster
                    {
                        Id = blog.Id,
                        MainId = blog.MainId,
                        SubId = blog.SubId,
                        BlogDescription = blog.BlogDescription,
                        Image = string.IsNullOrEmpty(blog.Image) ? null : $"{baseUrl}{blog.Image}",
                        Status = blog.Status,
                        CompId = blog.CompId,
                        BranchId = blog.BranchId,
                        Userid = blog.Userid,
                        YearId = blog.YearId,
                        Tittle = blog.Tittle,
                        Author = blog.Author,
                        CategoryName = categoryData
                            .Where(c => c.MainId == blog.MainId)
                            .Select(c => c.CategoryName)
                            .FirstOrDefault()
                    })
                    .ToList();

                int totalCount = await _context.BlogMaster
                    .Where(b => b.Status == 1)
                    .CountAsync();

                return (transformedPagedMetaTags, totalCount);
            }

        }
        #endregion

        #region GetBlogDetailsById
        public async Task<BlogDetails?> GetBlogDetailsById(int id)
        {
            var baseUrl = await _urlAddress.GetBaseUrlAsync();

            // Fetch BlogMaster details
            var blogMaster = await _context.BlogMaster
                .AsNoTracking()
                .FirstOrDefaultAsync(bm => bm.Id == id);

            if (blogMaster == null)
            {
                return null; // Handle the case where BlogMaster is not found
            }

            // Fetch related BlogTrans records
            var blogTrans = await _context.BlogTrans
                .AsNoTracking()
                .Where(bt => bt.Blogid == id)
                .ToListAsync();

            // Create BlogDetails object
            var blogDetails = new BlogDetails
            {
                BlogId = blogMaster.Id,
                Title = blogMaster.Tittle,
                SubId = blogMaster.SubId,
                MainId = blogMaster.MainId,
                BlogDescription = blogMaster.BlogDescription,
                image = blogMaster.Image != null ? $"{baseUrl}{blogMaster.Image}" : null,
                Status = blogMaster.Status,
                blogTrans = blogTrans,
                CategoryName = _context.ClassificationMaster.Where(x => x.MainId == blogMaster.MainId).FirstOrDefault().ClaasificationName
            };

            return blogDetails;
        }


		#endregion

		#region BlogReviewDetailsSave
		//public async Task<bool> CreateReviewAsync(BlogReviewDto reviewDto)
		//{
		//    var sql = @"
		//    INSERT INTO BlogTrans (
		//        Blogid,
		//        UserReview,
		//        EntryDate,
		//        Compid,
		//        Userid,
		//        Name,
		//        Email,
		//        Website,
		//        IsSaveNameEmailandWebsite,
		//        Comment
		//    )
		//    VALUES (
		//        @Blogid,
		//        @UserReview,
		//        @EntryDate,
		//        @Compid,
		//        @Userid,
		//        @Name,
		//        @Email,
		//        @Website,
		//        @IsSaveNameEmailandWebsite,
		//        @Comment
		//    );

		//    SELECT CAST(SCOPE_IDENTITY() AS INT)";

		//    var parameters = new[]
		//    {
		//    new SqlParameter("@Blogid", reviewDto.BlogId ?? (object)DBNull.Value),
		//    new SqlParameter("@UserReview", reviewDto.UserReview ?? (object)DBNull.Value),
		//    new SqlParameter("@EntryDate", DateTime.UtcNow),
		//    new SqlParameter("@Compid", 1),
		//    new SqlParameter("@Userid", reviewDto.UserId ?? (object)DBNull.Value),
		//    new SqlParameter("@Name", reviewDto.Name ?? (object)DBNull.Value),
		//    new SqlParameter("@Email", reviewDto.Email ?? (object)DBNull.Value),
		//    new SqlParameter("@Website", reviewDto.Website ?? (object)DBNull.Value),
		//    new SqlParameter("@IsSaveNameEmailandWebsite", reviewDto.IsSaveNameEmailandWebsite),
		//    new SqlParameter("@Comment", reviewDto.Comment ?? (object)DBNull.Value)
		//};

		//    try
		//    {
		//        var newId = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
		//        return newId > 0;
		//    }
		//    catch (Exception ex)
		//    {
		//        return false;
		//    }
		//}


		#endregion

		#region SaveBlogReviewDetails
		public async Task<bool> CreateReviewAsync(int? blogId, int? userReview, int? userId, string? name, string? email, 
            string? website, int isSaveNameEmailAndWebsite, string comment)
		{
			var sql = @"
    INSERT INTO BlogTrans (
        Blogid,
        UserReview,
        EntryDate,
        Compid,
        Userid,
        Name,
        Email,
        Website,
        IsSaveNameEmailandWebsite,
        Comment
    )
    VALUES (
        @Blogid,
        @UserReview,
        @EntryDate,
        @Compid,
        @Userid,
        @Name,
        @Email,
        @Website,
        @IsSaveNameEmailandWebsite,
        @Comment
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT)";

			var parameters = new[]
			{
		new SqlParameter("@Blogid", blogId ?? (object)DBNull.Value),
		new SqlParameter("@UserReview", userReview ?? (object)DBNull.Value),
		new SqlParameter("@EntryDate", DateTime.UtcNow),
		new SqlParameter("@Compid", 1),
		new SqlParameter("@Userid", userId ?? (object)DBNull.Value),
		new SqlParameter("@Name", name ?? (object)DBNull.Value),
		new SqlParameter("@Email", email ?? (object)DBNull.Value),
		new SqlParameter("@Website", website ?? (object)DBNull.Value),
		new SqlParameter("@IsSaveNameEmailandWebsite", isSaveNameEmailAndWebsite),
		new SqlParameter("@Comment", comment ?? (object)DBNull.Value)
	};

			try
			{
				var newId = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
				return newId > 0;
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);  
				return false;
			}
		}
		#endregion

		#region GetBlogReviewListByBlogId
		public async Task<List<BlogTrans>> GetBlogReviewListByBlogId(int BlogId)
        {
            var blogTrans = await _context.BlogTrans
                                          .AsNoTracking()
                                          .Where(bt => bt.Blogid == BlogId)
                                          .OrderByDescending(bt => bt.Id)
                                          .ToListAsync();
            return blogTrans;
        }

        #endregion

    }
}
