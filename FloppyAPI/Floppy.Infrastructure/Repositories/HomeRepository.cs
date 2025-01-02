using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class HomeRepository :IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlAddressRepositary _urlAddressRepositary;
        private readonly ILogger<HomeRepository> _logger;   
        public HomeRepository(ApplicationDbContext context, IUrlAddressRepositary urlAddressRepositary,ILogger<HomeRepository> logger)
        {
            _context = context;
            _urlAddressRepositary = urlAddressRepositary;
            _logger = logger;
        }
		#region FetchAllDynamicDataForHomePage
		public async Task<Dictionary<string, List<Dictionary<string, object>>>> GetAllDynamicDataForHomePageAsync()
        {
            var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
            var sql = @"SELECT Id, Image, URL, Tittle, Status, Seqno, Type, BranchId, Compid, UserId, Yearid FROM HomePageSlider WHERE Status = 1";
            var results = await _context.HomePageSlider
                .FromSqlRaw(sql)
                .AsNoTracking() 
                .ToListAsync();
            var groupedResult = results
                .GroupBy(item => item.Type ?? "Unknown")
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .OrderBy(item => item.Seqno) 
                        .Select(item => new Dictionary<string, object>
                        {
                            { "Image", string.IsNullOrEmpty(item.Image) ? null : $"{baseUrl}{item.Image}" },
                            { "URL", item.URL },
                            { "Tittle", item.Tittle },
                            { "Status", item.Status },
                            { "Seqno", item.Seqno }
                        }).ToList() 
                );

            return groupedResult;
        }
		#endregion

		#region FetchAllItemList
		public async Task<List<Item>> GetAllItemList()
        {
            var baseurl = await _urlAddressRepositary.GetBaseUrlAsync();

            var items = _context.Itemmaster
                                .Where(x => x.ShowOnDastboard == 1)
                                .GroupJoin(
                                    _context.PackageTrans.Where(p => p.ProductName == "Image"),
                                    item => item.id,
                                    packageTrans => packageTrans.TransId,
                                    (item, packageTrans) => new { item, packageTrans = packageTrans.FirstOrDefault() }
                                )
                                .ToList();
            items.ForEach(x =>
            {
                if (x.packageTrans != null)
                {
                    x.item.imagepath = $"{baseurl}{x.packageTrans.ProductValue}";
                }
            });

            return items.Select(x => x.item).ToList();
        }
		#endregion

		#region GetCategorieswithSubcatgories
		public async Task<List<CategoryDto>> GetCategoriesWithSubcategoriesAsync()
        {
            var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
			var categories = await _context.ClassificationMaster
	                        .AsNoTracking()
	                        .ToListAsync();

			var subcategories = await _context.ClassificationTrans
	                        .AsNoTracking()
	                        .ToListAsync();
			// Join the categories and subcategories manually using LINQ
			var result = categories.Select(category => new CategoryDto
            {
                MainId = category.MainId,
                ClassificationName = category.ClaasificationName,
                Categoryseourl= category.Categoryseourl,
                Image = string.IsNullOrEmpty(category.Image) ? null : $"{baseUrl}{category.Image}",
                Description = category.Description,
                Url = category.Url,
                Status = category.Status,
                ShowOnDashboard = category.ShowOnDastboard,
                Subcategories = subcategories
                    .Where(sub => sub.MainId == category.MainId)
                    .Select(sub => new SubcategoryDto
                    {
                        MainId = sub.MainId,
                        SubId = sub.SubId,
                        SubClassificationName = sub.SubClassificationName,
                        NewUrl = sub.Newrurl,
                        Image = string.IsNullOrEmpty(sub.Image) ? null : $"{baseUrl}{sub.Image}",
                        Url = sub.Url,
                        GoogleName = sub.Googelname,
                        Status = sub.Status,
                        ShowOnDashboard = sub.ShowOnDastboard
                    })
                    .ToList()
            })
            .ToList();

            return result;
        }
		#endregion

		#region FetchAreaList
		public async Task<List<string>> GetAreasList()
        {
            var sql = @"SELECT * FROM VendorRegistration";

            var results = await _context.VendorRegistration
                .FromSqlRaw(sql)
                .AsNoTracking()
                .ToListAsync();

            // Select distinct areas, remove null or empty values, trim whitespace, and exclude entries containing commas
            var filteredResults = results
                .Select(item => item.Area?.Trim()) 
                .Where(area => !string.IsNullOrEmpty(area) && !area.Contains(",")) 
                .Distinct() 
                .ToList();

            return filteredResults; // Return the filtered list of areas
        }
		#endregion

		#region GetvendorRegistrationList
		public async Task<List<CityModel>> GetVendorRegistrationList()
        {
            var data = await (from vendor in _context.VendorRegistration.AsNoTracking()
                              join item in _context.Itemmaster.AsNoTracking()
                              on vendor.id equals item.vendorid
                              where !string.IsNullOrEmpty(vendor.CityName) &&
                                    !string.IsNullOrEmpty(vendor.Area) &&
                                    !vendor.Area.Contains(",") &&
                                    !string.IsNullOrEmpty(item.subgroupname)
                              select new
                              {
                                  vendor.CityName,
                                  vendor.Area,
                                  item.subgroupname,
                                  item.subgroupid
                              })
                              .ToListAsync();

            var cityModels = data
                .GroupBy(x => x.CityName?.Trim() ?? string.Empty)
                .Select(cityGroup => new CityModel
                {
                    CityName = cityGroup.Key,
                    Areas = cityGroup
                        .GroupBy(x => x.Area?.Trim() ?? string.Empty)
                        .Select(areaGroup => new AreaModel
                        {
                            AreaName = areaGroup.Key,
                            Subgroups = areaGroup
                                .GroupBy(x => new { SubgroupName = x.subgroupname?.Trim() ?? string.Empty, x.subgroupid })
                                .Select(subgroupGroup => new SubgroupModel
                                {
                                    SubgroupName = subgroupGroup.Key.SubgroupName,
                                    SubgroupId = subgroupGroup.Key.subgroupid
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return cityModels;
        }
		#endregion

		#region  Location with Latitude and Longitude
		//public async Task<List<CityModel>> GetVendorRegistrationList()
		//{
		//    // Execute the query to get data with minimal processing
		//    var data = await (from vendor in _context.VendorRegistration.AsNoTracking()
		//                      join item in _context.Itemmaster.AsNoTracking()
		//                      on vendor.id equals item.vendorid
		//                      where !string.IsNullOrEmpty(vendor.CityName) &&
		//                            !string.IsNullOrEmpty(vendor.Area) &&
		//                            !vendor.Area.Contains(",") &&
		//                            vendor.latitude != null &&   // Ensure latitude is not null
		//                            vendor.longitude != null     // Ensure longitude is not null
		//                      select new
		//                      {
		//                          vendor.CityName,
		//                          vendor.Area,
		//                          vendor.latitude,
		//                          vendor.longitude,
		//                          item.subgroupname,
		//                          item.subgroupid
		//                      })
		//                      .ToListAsync();

		//    // Process data in memory after retrieval
		//    var cityModels = data
		//        .GroupBy(x => x.CityName.Trim())
		//        .Select(cityGroup => new CityModel
		//        {
		//            CityName = cityGroup.Key,
		//            Areas = cityGroup
		//                .GroupBy(x => x.Area.Trim())
		//                .Select(areaGroup => new AreaModel
		//                {
		//                    AreaName = areaGroup.Key,
		//                    Latitude = areaGroup.FirstOrDefault(x => x.latitude != null)?.latitude ?? "",  // Safely handle null latitude
		//                    Longitude = areaGroup.FirstOrDefault(x => x.longitude != null)?.longitude ?? "", // Safely handle null longitude
		//                    Subgroups = areaGroup
		//                        .GroupBy(x => new { x.subgroupname, x.subgroupid })
		//                        .Select(subgroupGroup => new SubgroupModel
		//                        {
		//                            SubgroupName = subgroupGroup.Key.subgroupname.Trim(),
		//                            SubgroupId = subgroupGroup.Key.subgroupid
		//                        })
		//                        .ToList()
		//                })
		//                .ToList()
		//        })
		//        .ToList();

		//    return cityModels;
		//}
		#endregion


	}
}
