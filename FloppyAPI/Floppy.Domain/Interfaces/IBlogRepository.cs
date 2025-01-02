using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IBlogRepository
    {
        Task<(List<BlogMaster> MetaTags, int TotalCount)> GetBlogListAsync(int startIndex, int pageSize);
        Task<BlogDetails?> GetBlogDetailsById(int id);
        //Task<bool> CreateReviewAsync(BlogReviewDto reviewDto);
        Task<bool> CreateReviewAsync(int? blogId, int? userReview, int? userId, string? name, string? email,
                                    string? website, int isSaveNameEmailAndWebsite, string comment);

		Task<List<BlogTrans>> GetBlogReviewListByBlogId(int BlogId);
    }
}
