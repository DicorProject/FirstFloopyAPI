using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class MetaTagRepository : IMetaTagRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MetaTagRepository> _logger;
        public MetaTagRepository(ApplicationDbContext context, ILogger<MetaTagRepository> logger)
        {
            _context = context;
            _logger = logger;   
        }

        #region GetMetatagList
        public async Task<List<Metatag>> FetchListOfMetatags()
        {
            var metatagsData = await _context.MetaTags.ToListAsync();
            return metatagsData;
        }
        #endregion

        #region GetMetagByPageName
        public async Task<Metatag> GetMetagDetailsBypageName(string pagename)
        {
			var formattedPageName = pagename.ToLower().Replace(" ", "");

			var metatag = await _context.MetaTags
										.FirstOrDefaultAsync(x => x.Page.ToLower().Replace(" ", "").Contains(formattedPageName));
			return metatag;
        }
        #endregion
    }
}
