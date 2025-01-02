using Floppy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class UrlAddressRepository : IUrlAddressRepositary
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UrlAddressRepository> _logger;
        public UrlAddressRepository(ApplicationDbContext context,ILogger<UrlAddressRepository> logger)
        {
            _context = context;
            _logger = logger;   
        }

		#region GetBaseUrl
		public async Task<string> GetBaseUrlAsync()
		{
			try
			{
				var baseUrl = await _context.url
					.AsNoTracking()
					.Where(u => u.Type == "baseUrl")
					.Select(u => u.UrlAddress)
					.FirstOrDefaultAsync();

				return baseUrl ?? string.Empty;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving the base URL.");
				return string.Empty;
			}
		}
		#endregion

	}
}
