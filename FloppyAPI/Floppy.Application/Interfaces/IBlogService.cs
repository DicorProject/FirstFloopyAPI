using Floppy.Application.DTOs.Response;
using Floppy.Application.Models.Request;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IBlogService
    {
        Task<ApiResponse<BlogCountwithList>> FetchBlogList(int startIndex, int PageSize);
        Task<ApiResponse<BlogDetails>> GetBlogDetailsByPageId(int PageId);
        Task<ApiResponse<bool>> saveBlogReview(BlogReviewInputRequest request);
        Task<ApiResponse<List<BlogTrans>>> GetReviewListByBlogId(int BlogId);
    }
}
