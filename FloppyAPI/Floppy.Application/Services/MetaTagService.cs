using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class MetaTagService: IMetaTagService
    {
        private readonly IMetaTagRepository _metaTagRepository;
        private readonly ILogger<MetaTagService> _logger;   
        public MetaTagService(IMetaTagRepository metaTagRepository,ILogger<MetaTagService> logger)
        {
            _metaTagRepository = metaTagRepository;
            _logger = logger;   
        }
        #region GetMetagList
        public async Task<ApiResponse<List<Metatag>>> GetMetagLists()
        {
            var response = new ApiResponse<List<Metatag>>();

            try
            {
                var metatags = await _metaTagRepository.FetchListOfMetatags();

                if (metatags != null && metatags.Any())
                {
                    response.Success = true;
                    response.Message = "Metatags retrieved successfully.";
                    response.Data = metatags;
                }
                else
                {
                    response.Success = true;
                    response.Message = "No metatags found.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving metatags: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion

        #region GetMetagByPageName
        public async Task<ApiResponse<Metatag>> GetMetagByPageName(string pageName)
        {
            var response = new ApiResponse<Metatag>();

            try
            {
                var metatags = await _metaTagRepository.GetMetagDetailsBypageName(pageName);

                if (metatags != null)
                {
                    response.Success = true;
                    response.Message = $"Metatags for page '{pageName}' retrieved successfully.";
                    response.Data = metatags;
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No metatags found for page '{pageName}'.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving metatags for page '{pageName}': {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion

    }
}
