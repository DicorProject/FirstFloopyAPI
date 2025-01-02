using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface ITestimonialRepository
    {
        Task<List<Testimonial>> GetAllTestimonialsAsync();
    }
}
