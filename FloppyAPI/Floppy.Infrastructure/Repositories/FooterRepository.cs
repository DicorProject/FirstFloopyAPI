using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class FooterRepository : IFooterRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FooterRepository> _logger; 
        public FooterRepository(ApplicationDbContext context,ILogger<FooterRepository> logger)
        {
            _context = context;
            _logger = logger;   
        }

        #region FetchFooterParameterList
        public async Task<List<FooterTypeGroupDto>> GetAllFootersWithParametersGroupedByTypeAsync()
        {
            try
            {
                var footers = await _context.Footer
                            .AsNoTracking()
                            .ToListAsync();
                var footerTypes = footers
                    .Where(f => f.Type.HasValue)
                    .Select(f => f.Type.Value)
                    .Distinct()
                    .ToList();
                List<Parameter> parameters = new List<Parameter>();
                if (footerTypes.Any())
                {
                    var parameterIds = string.Join(",", footerTypes);
                    var parameterQuery = $@"SELECT * FROM [dbo].[Parameter] WHERE Paraid IN ({parameterIds})";

                    parameters = await _context.Parameter
                        .FromSqlRaw(parameterQuery)
                        .ToListAsync();
                }
                var groupedResult = footerTypes
                    .Select(type => new FooterTypeGroupDto
                    {
                        Type = type,
                        // Get parameter details for this type
                        ParameterName = parameters.FirstOrDefault(p => p.Paraid == type)?.ParameterName,
                        ParameterDetail = parameters.FirstOrDefault(p => p.Paraid == type)?.ParameterDetail,
                        Footers = footers
                            .Where(f => f.Type == type)
                            .Select(f => new FooterWithParametersDto
                            {
                                Id = f.Id,
                                SeqNo = f.SeqNo,
                                Tittle = f.Tittle,
                                URL = f.URL,
                                YearId = f.YearId
                            })
                            .ToList()
                    })
                    .ToList();

                return groupedResult;
            }
            catch(Exception ex)
            {
                return new List<FooterTypeGroupDto>();
            }
        }
		#endregion
	}
}
