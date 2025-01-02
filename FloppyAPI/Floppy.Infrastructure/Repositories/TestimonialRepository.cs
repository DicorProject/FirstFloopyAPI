using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class TestimonialRepository : ITestimonialRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlAddressRepositary _urlAddressRepositary;
        private readonly ILogger<TestimonialRepository> _logger;
        public TestimonialRepository(ApplicationDbContext context, IUrlAddressRepositary urlAddressRepositary,ILogger<TestimonialRepository> logger)
        {
            _context = context;
            _urlAddressRepositary = urlAddressRepositary;
            _logger = logger;   
        }
		#region FetchTestimonialsList
		public async Task<List<Testimonial>> GetAllTestimonialsAsync()
        {
            var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
			var testimonials = await _context.TestimonialMaster
											 .AsNoTracking()
											 .Where(t => t.Status == 1)
											 .ToListAsync();
			var testimonialsWithImages = testimonials.Select(item => new Testimonial
            {
                Id = item.Id,
                CustumerName = item.CustumerName,
                CustomerDesigination = item.CustomerDesigination,
                Image = string.IsNullOrEmpty(item.Image) ? null : $"{baseUrl}{item.Image}",
                Rate = item.Rate,
                Review = item.Review,
                Status = item.Status,
                Compid = item.Compid,
                UserId = item.UserId,
                BranchId = item.BranchId,
                YearId = item.YearId
            }).ToList();

            return testimonialsWithImages;
        }
		#endregion
	}
}
