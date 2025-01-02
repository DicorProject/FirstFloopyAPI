using Floppy.Application.DTOs.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface ITestimonialService
    {
        Task<ApiResponse<List<Testimonial>>> GetTestimonialList();
    }
}
