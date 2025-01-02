using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class TestimonialService : ITestimonialService
    {
        private readonly ITestimonialRepository _repository;
        private readonly ILogger<TestimonialService> _logger;   
        public TestimonialService(ITestimonialRepository repository, ILogger<TestimonialService> logger)
        {
            _repository = repository;   
            _logger = logger;   
        }
        #region Fetch All List of Testimonials
        public async Task<ApiResponse<List<Testimonial>>> GetTestimonialList()
        {
            var response = new ApiResponse<List<Testimonial>>();

            try
            {
                // Fetch all testimonials where status is 1 from the repository
                var testimonialList = await _repository.GetAllTestimonialsAsync();

                if (testimonialList != null && testimonialList.Any())
                {
                    response.Success = true;
                    response.Message = "Testimonial list retrieved successfully.";
                    response.Data = testimonialList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No testimonials found.";
                    response.Data = new List<Testimonial>();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion
    }
}
