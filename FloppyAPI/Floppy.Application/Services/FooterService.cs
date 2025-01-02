using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class FooterService :IFooterService
    {
        private  readonly IFooterRepository _footerRepository;
        private readonly ILogger<FooterService> _logger;        
        public FooterService(IFooterRepository footerRepository, ILogger<FooterService> logger)
        {
            _footerRepository = footerRepository;
            _logger = logger;   
        }
        #region GetAllFooterData
        public async Task<ApiResponse<List<FooterTypeGroupDto>>> GetAllFootersAsync()
        {
            var response = new ApiResponse<List<FooterTypeGroupDto>>();
            try
            {
                var footerList = await _footerRepository.GetAllFootersWithParametersGroupedByTypeAsync();
                if (footerList != null && footerList.Any())
                {
                    response.Success = true;
                    response.Message = "Footer list retrieved successfully.";
                    response.Data = footerList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No footers found.";
                    response.Data = null;
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
