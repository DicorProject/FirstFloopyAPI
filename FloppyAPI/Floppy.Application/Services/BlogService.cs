using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class BlogService:IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ILogger<BlogService> _logger;
        public BlogService(IBlogRepository blogRepository,ILogger<BlogService> logger)
        {
            _blogRepository = blogRepository;
            _logger = logger;   
        }
        #region FetchBlogList
        public async Task<ApiResponse<BlogCountwithList>> FetchBlogList(int startIndex, int PageSize)
        {
            var response = new ApiResponse<BlogCountwithList>();

            try
            {
                // Assuming GetMetaTagsList returns a list of blogs
                var bloglist = await _blogRepository.GetBlogListAsync(startIndex,PageSize);

                if (bloglist.MetaTags != null)
                {
                    response.Success = true;
                    response.Message = "Fetched blog list successfully.";
                    response.Data = new BlogCountwithList
                    {
                        blogs=bloglist.MetaTags,
                        TotalBlogs= bloglist.TotalCount,
                    };
                }
                else
                {
                    response.Success = false;
                    response.Message = "No blogs found.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while fetching the blog list");
				response.Success = false;
                response.Message = $"An error occurred while fetching the blog list: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion

        #region GetBlogDetailsByPageId
        public async Task<ApiResponse<BlogDetails>> GetBlogDetailsByPageId(int pageId)
        {
            var response = new ApiResponse<BlogDetails>();

            try
            {
                var blog = await _blogRepository.GetBlogDetailsById(pageId);

                if (blog != null)
                {
                    response.Success = true;
                    response.Message = "Blog details retrieved successfully.";
                    response.Data = blog;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No blog found with the specified name.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while retrieving the blog details");
				response.Success = false;
                response.Message = $"An error occurred while retrieving the blog details: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion

        #region SaveBlogReviewDetails
        public async Task<ApiResponse<bool>> saveBlogReview(BlogReviewInputRequest request)
        {
            var response = new ApiResponse<bool>();

            try
            {
				// Assuming CreateReviewAsync returns the new Id
				bool isSaved = await _blogRepository.CreateReviewAsync(
						   request.BlogId,
						   request.UserReview,
						   request.UserId,
						   request.Name,
						   request.Email,
						   request.Website,
						   request.IsSaveNameEmailandWebsite,
						   request.Comment
					   );

				if (isSaved)
                {
                    response.Success = true;
                    response.Message = "Blog review saved successfully.";
                    response.Data = isSaved;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to save the blog review.";
                    response.Data = false;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while saving the blog review");
				response.Success = false;
                response.Message = $"An error occurred while saving the blog review: {ex.Message}";
                response.Data=false;
            }

            return response;
        }
        #endregion

        #region GetReviewListByBlogId 
        public async Task<ApiResponse<List<BlogTrans>>> GetReviewListByBlogId(int BlogId)
        {
            var response = new ApiResponse<List<BlogTrans>>();

            try
            {
                // Fetch the blog review list by BlogId
                var blogReviews = await _blogRepository.GetBlogReviewListByBlogId(BlogId);

                if (blogReviews != null && blogReviews.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Blog reviews retrieved successfully.";
                    response.Data = blogReviews;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No blog reviews found.";
                    response.Data = new List<BlogTrans>();
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while retrieving the blog reviews");
				response.Success = false;
                response.Message = $"An error occurred while retrieving the blog reviews: {ex.Message}";
                response.Data = new List<BlogTrans>();
            }

            return response;
        }

        #endregion
    }
}
