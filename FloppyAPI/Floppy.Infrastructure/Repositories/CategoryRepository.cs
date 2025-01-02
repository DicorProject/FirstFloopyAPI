using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlAddressRepositary _urlAddressRepositary;
        private readonly ILogger<CategoryRepository> _logger;
        public CategoryRepository(ApplicationDbContext context, IUrlAddressRepositary urlAddressRepositary, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _urlAddressRepositary = urlAddressRepositary;
            _logger = logger;

        }
        #region FetchCategoriesList
        public async Task<List<object>> GetAllCategoriesAsync()
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
                var results = await _context.ClassificationMaster
                              .Where(c => c.Status == 1)
                              .AsNoTracking()
                               .ToListAsync();
                // Map results to anonymous type
                var filteredResults = results.Select(item => new
                {
                    ClassificationName = item.ClaasificationName,
                    MainId = item.MainId,
                    Image = string.IsNullOrEmpty(item.Image) ? null : $"{baseUrl}{item.Image}",
                    Description = item.Description,
                    Url = item.Url,
                    Status = item.Status,
                    ShowOnDashboard = item.ShowOnDastboard,
                    Categoryseourl = item.Categoryseourl,

                }).Cast<object>().ToList();

                return filteredResults;
            }
            catch (Exception ex)
            {
                return new List<object>();
            }
        }
        #endregion


        #region FetchServicesList
        public async Task<List<Dictionary<string, object>>> GetAllServicesAsync(int categoryId)
        {
            try
            {
                var results = await _context.services
                                   .Where(c => c.MainCatId == categoryId)
                                   .AsNoTracking()
                                   .ToListAsync();

                // Group by ServiceName and collect SubCatIds as comma-separated strings
                var filteredResults = results
                    .GroupBy(item => new { item.ServiceName, item.MainCatId, item.ServiceSeoUrl })
                    .Select(g => new Dictionary<string, object>
                    {
                    { "MainId", g.Key.MainCatId },
                    { "serviceName", g.Key.ServiceName },
                    { "SubIds", string.Join(",", g.Select(x => x.SubCatId).Distinct()) },
                    { "serviceId", g.First().Id },
                    { "ServiceSeoUrl",g.Key.ServiceSeoUrl }
                    })
                    .ToList();

                return filteredResults;
            }
            catch (Exception ex)
            {
                return new List<Dictionary<string, object>>();

            }
        }
        #endregion


        #region FetchSubCategoriesListByCategoryId
        public async Task<List<Dictionary<string, object>>> GetAllSubCategoriesByCategoryIdAsync(int categoryId)
        {
            try
            {
                var CategoryName = _context.ClassificationMaster.Where(x => x.MainId == categoryId).FirstOrDefault();
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
                var results = await _context.ClassificationTrans
                               .Where(c => c.MainId == categoryId)
                                .AsNoTracking()
                                .ToListAsync();

                var filteredResults = results.Select(item => new Dictionary<string, object>
                {
                { "MainId", item.MainId },
                { "CategoryName",CategoryName.ClaasificationName },
                { "Categoryseourl", CategoryName.Categoryseourl},
                { "SubId", item.SubId },
                { "SubClassificationName", item.SubClassificationName },
                { "NewUrl", item.Newrurl },
                { "Image", string.IsNullOrEmpty(item.Image) ? null : $"{baseUrl}{item.Image}" },
                { "Url", item.Url },
                { "GoogleName", item.Googelname },
                {"Status",item.Status },
                {"ShowOnDashboard",item.ShowOnDastboard }
                }).ToList();

                return filteredResults;
            }
            catch (Exception ex)
            {
                return new List<Dictionary<string, object>>();
            }
        }
        #endregion


        #region FetchAllItemsListWithDetails
        public async Task<(List<Item> Items, int TotalCount)> GetAllItemListWithDetails(List<int> subids, List<string> serviceNames, int mainid, int startIndex = 0, int pageSize = 0)
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
                var sql = @"
                    WITH RankedItems AS (
                        SELECT im.*, pt.ProductValue, vr.Status,
                        ROW_NUMBER() OVER (PARTITION BY im.vendorid ORDER BY im.rate ASC) AS RowNum
                        FROM Itemmaster im
                        LEFT JOIN PackageTrans pt
                        ON im.itemid = pt.TransId AND pt.ProductName = 'image'
                        LEFT JOIN VendorRegistration vr
                        ON im.vendorid = vr.id
                        WHERE im.maingroupid = @MainId 
                        AND im.websitestatus =1
                        AND vr.Status != 'Pending'";

                if (subids != null && subids.Count > 0)
                {
                    var subIdsString = string.Join(",", subids);
                    sql += @" AND im.subgroupid IN (" + subIdsString + @")";
                }
                if (serviceNames != null && serviceNames.Count > 0)
                {
                    var serviceNamesLowercase = string.Join(",", serviceNames.Select(name => "'" + name.ToLower() + "'"));
                    sql += @" AND LOWER(im.ItemName) IN (" + serviceNamesLowercase + @")";
                }
                sql += @")SELECT * FROM RankedItems  WHERE RowNum = 1";

                // Add pagination only if startIndex and pageSize are non-zero
                if (startIndex > 0 || pageSize > 0)
                {
                    sql += @"ORDER BY itemid OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY";
                }

                // Retrieve items based on the SQL query
                var items = await _context.Itemmaster
                    .FromSqlRaw(sql, new[]
                    {
                    new SqlParameter("@MainId", mainid),
                    new SqlParameter("@StartIndex", startIndex),
                    new SqlParameter("@PageSize", pageSize)
                    })
                    .AsNoTracking()
                    .ToListAsync();
                var serviceIds = items.Select(i => i.serviceid).ToList(); // Extract the service IDs from 'items'.

                // Use a parameterized query to avoid SQL injection
                var query = @" SELECT Id, ServiceSeoUrl FROM services WHERE Id IN ({0})";

                var sqlQuery = string.Format(query, string.Join(", ", serviceIds));

                // Execute the raw SQL query
                var services = await _context.services
                    .FromSqlRaw(sqlQuery)
                    .Select(s => new { s.Id, s.ServiceSeoUrl }) // Project the columns needed
                    .ToListAsync();

                // Convert to dictionary
                var servicesMap = services.ToDictionary(s => s.Id, s => s.ServiceSeoUrl);

                // Fetch related packages
                var packages = await _context.PackageTrans
                           .AsNoTracking()
                           .ToListAsync();

                // Aggregate image paths into a dictionary
                var imagePaths = packages
                    .Where(p => !string.IsNullOrEmpty(p.ProductValue) && p.ProductName.Contains("Image", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(p => p.TransId)
                    .ToDictionary(
                        g => g.Key,
                        g => string.Join(",", g.Select(p => $"{baseUrl}{p.ProductValue}"))
                    );

                var ratesByTransId = packages
                        .Where(pt => !string.IsNullOrEmpty(pt.ProductValue) &&
                                     pt.ProductName.Contains("Rate", StringComparison.OrdinalIgnoreCase))
                        .GroupBy(pt => pt.TransId)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(pt => new Rate
                            {
                                Key = pt.ProductName,
                                Value = pt.ProductValue
                            }).ToList()
                        );

                // Process items to set image paths and convert rates
                items.ForEach(item =>
                {
                    if (item.serviceid.HasValue && servicesMap.TryGetValue(item.serviceid.Value, out var seoUrl))
                    {
                        item.ServiceSeoUrl = seoUrl;
                    }
                    if (imagePaths.TryGetValue(Convert.ToInt32(item.itemid), out var imagePath))
                    {
                        item.imagepath = imagePath;
                    }
                    else
                    {
                        item.imagepath = string.IsNullOrEmpty(item.imagepath) ? null : $"{baseUrl}{item.imagepath}";
                    }

                    if (!string.IsNullOrEmpty(item.Rate))
                    {
                        string numericPart = new string(item.Rate.Where(c => char.IsDigit(c) || c == '.').ToArray()).Trim();

                        // Ensure there's something numeric to parse
                        if (!string.IsNullOrEmpty(numericPart) && double.TryParse(numericPart, out double parsedPrice))
                        {
                            item.Price = parsedPrice;
                            item.ItemRate = parsedPrice;
                        }
                    }
                    if (ratesByTransId.TryGetValue(Convert.ToInt32(item.itemid), out var rates))
                    {
                        item.RateList = rates;
                    }
                    else
                    {
                        item.RateList = new List<Rate>();
                    }
                    item.ItemRateForDisplay = item.Rate;
                });

                // Count total items with the same subids filter
                var rawsql = @"
                           WITH RankedItems AS (
                                SELECT im.*, pt.ProductValue, vr.Status,
                                        ROW_NUMBER() OVER (PARTITION BY im.vendorid ORDER BY im.rate ASC) AS RowNum
                                        FROM Itemmaster im
                                        LEFT JOIN PackageTrans pt
                                        ON im.itemid = pt.TransId AND pt.ProductName = 'image'
                                        LEFT JOIN VendorRegistration vr
                                        ON im.vendorid = vr.id
                                        WHERE im.maingroupid = @MainId 
                                        AND im.websitestatus =1
                                        AND vr.Status != 'Pending'";
                if (subids != null && subids.Count > 0)
                {
                    var subIdsString = string.Join(",", subids);
                    rawsql += @" AND im.subgroupid IN (" + subIdsString + @")";
                }
                if (serviceNames != null && serviceNames.Count > 0)
                {
                    var serviceNamesLowercase = string.Join(",", serviceNames.Select(name => "'" + name.ToLower() + "'"));
                    rawsql += @" AND LOWER(im.ItemName) IN (" + serviceNamesLowercase + @")";
                }
                rawsql += @") SELECT * FROM RankedItems WHERE RowNum = 1";
                var itemscount = await _context.Itemmaster
                    .FromSqlRaw(rawsql, new[]
                    {
                    new SqlParameter("@MainId", mainid)
                    })
                    .AsNoTracking()
                    .ToListAsync();

                var totalCount = itemscount.Count();

                return (items, totalCount);
            }
            catch (Exception ex)
            {
                return (Items: new List<Item>(), TotalCount: 0);
            }
        }
        #endregion


        #region GetItemsById
        public async Task<ItemDeatsils> GetItemByIdAsync(int id)
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
                var taxfee = await _context.Tax.Select(t => t.Fees).FirstOrDefaultAsync();
                // Fetch item details from Itemmaster
                var item = await _context.Itemmaster
                    .Where(i => i.id == id)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();

                if (item == null)
                {
                    return null; // Item not found
                }

                // Fetch related package master details
                var packages = await _context.Packagemaster
                    .Where(p => p.productid == id)
                    .AsNoTracking()
                    .ToListAsync();

                // Fetch related package transactions
                var packageTrans = await _context.PackageTrans
                    .Where(pt => pt.TransId == id)
                    .AsNoTracking()
                    .ToListAsync();

                // Separate image-related and other product transactions
                var imageProducts = packageTrans
                                    .Where(pt => !string.IsNullOrEmpty(pt.ProductValue) &&
                                    !string.IsNullOrEmpty(pt.ProductName) &&
                                    pt.ProductName.Contains("Image", StringComparison.OrdinalIgnoreCase))
                                    .Select(pt => new Packagetransdto
                                    {
                                        ProductName = pt.ProductName,
                                        ProductValue = $"{baseUrl}{pt.ProductValue}" // Append base URL for images
                                    }).ToList();

                var otherProducts = packageTrans
                    .Where(pt => !string.IsNullOrEmpty(pt.ProductValue) &&
                                 (string.IsNullOrEmpty(pt.ProductName) ||
                                  !pt.ProductName.Contains("Image", StringComparison.OrdinalIgnoreCase)))
                    .Select(pt => new Packagetransdto
                    {
                        ProductName = pt.ProductName,
                        ProductValue = pt.ProductValue // No base URL added
                    })
                    .ToList();


                // Combine image-related and other products into one list
                var combinedProductDetails = imageProducts.Concat(otherProducts).ToList();

                if (imageProducts.Any())
                {
                    item.imagepath = string.Join(",", imageProducts.Select(p => p.ProductValue));
                }
                // Construct the ItemDetails object with package master and package transactions data
                var itemDetails = new ItemDeatsils
                {
                    itemid = item.id,
                    imagepath = item.imagepath,
                    ItemName = item.ItemName,
                    ItemCode = item.ItemCode,
                    serviceimages = item.serviceimages,
                    Rate = item.Rate,
                    id = item.id,
                    productid = item.productid,
                    vendorid = item.vendorid,
                    vendorname = item.vendorname,
                    maingroupid = item.maingroupid,
                    maingroupname = item.maingroupname,
                    subgroupid = item.subgroupid,
                    subgroupname = item.subgroupname,
                    servicename = item.servicename,
                    servicevalue = item.servicevalue,
                    specication = item.specication,
                    specicationname = item.specicationname,
                    ServiceSeoUrl = _context.services.Where(x => x.Id == item.serviceid).FirstOrDefault()?.ServiceSeoUrl,
                    ShowOnDastboard = item.ShowOnDastboard,
                    ItemRate = double.TryParse(new string(item.Rate.Where(c => char.IsDigit(c) || c == '.').ToArray()).Trim(), out double parsedPrice) ? parsedPrice : 0,
                    GroupType = _context.ClassificationMaster.Where(x => x.MainId == item.maingroupid).FirstOrDefault().GroupType,
                    ItemRateForDisplay = item.Rate,
                    Tax = taxfee,
                    Quantity = 1,
                    packagemasters = (await _context.Packagemaster
                            .Where(p => p.productid == id)
                            .AsNoTracking()
                            .ToListAsync())
                            .Select(p => new packagemasterdto
                            {
                                productid = p.productid,
                                vendorid = p.vendorid,
                                vendorname = p.vendorname,
                                servicename = p.servicename,
                                servicevalue = p.servicevalue,
                                specication = p.specication,
                                specicationname = p.specicationname
                            }).ToList(),
                    packageTrans = combinedProductDetails
                };

                return itemDetails;
            }
            catch (Exception ex)
            {
                return new ItemDeatsils();
            }
        }
        #endregion


        #region GetCategoriesWithSubCategoriesAndFirstPrice
        public async Task<List<CategoryData>> GetCategoriesWithSubCategoriesAndFirstPriceAsync(int CategoryId)
        {
            try
            {
                // Step 1: Get all categories
                var sqlCategories = CategoryId == 0
                    ? @"SELECT * FROM ClassificationMaster WHERE Status = 1"
                    : @"SELECT * FROM ClassificationMaster WHERE MainId = @CategoryId AND Status = 1";

                var categoryParams = new SqlParameter("@CategoryId", CategoryId);
                var categories = CategoryId == 0
                    ? await _context.ClassificationMaster
                        .FromSqlRaw(sqlCategories)
                        .AsNoTracking()
                        .ToListAsync()
                    : await _context.ClassificationMaster
                        .FromSqlRaw(sqlCategories, categoryParams)
                        .AsNoTracking()
                        .ToListAsync();

                // Initialize the result list
                var result = new List<CategoryData>();

                foreach (var category in categories)
                {
                    // Fetch rate card data for the current category
                    var sqlRateCard = @"
                    SELECT [Rate], [Sparepartname] 
                    FROM RateCard 
                    WHERE maincatid = @MainCatId";

                    var rateCardParams = new[]
                    {
                        new SqlParameter("@MainCatId", category.MainId)
                    };

                    var rateCardDataList = await _context.RateCard
                        .FromSqlRaw(sqlRateCard, rateCardParams)
                        .AsNoTracking()
                        .Select(rateCard => new
                        {
                            rateCard.Rate,
                            rateCard.Sparepartname
                        })
                        .ToListAsync();

                    // Prepare subcategory list for this category
                    var subCategoryList = rateCardDataList.Select(rateCard => new SubCategoryData
                    {
                        SubCategoryName = rateCard.Sparepartname ?? string.Empty, // Default empty string
                        FirstPrice = rateCard.Rate?.ToString() ?? "0"           // Default price as "0"
                    }).ToList();

                    // Add category data to the result
                    result.Add(new CategoryData
                    {
                        CategoryName = category.ClaasificationName,
                        SubCategories = subCategoryList
                    });
                }


                return result;
            }
            catch (Exception ex)
            {
                return new List<CategoryData>();
            }
        }
        #endregion


        #region GetItemsBySubGroupId
        public async Task<List<Item>> GetItemsBySubGroupIdAsync(int subgroupId, string location)
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();

                if (subgroupId <= 0 || string.IsNullOrWhiteSpace(location))
                {
                    return new List<Item>();
                }
                var packages = await _context.PackageTrans
                .AsNoTracking()
                .ToListAsync();

                // Aggregate image paths into a dictionary
                var imagePaths = packages
                    .Where(p => !string.IsNullOrEmpty(p.ProductValue) && p.ProductName.Contains("Image", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(p => p.TransId)
                    .ToDictionary(
                        g => g.Key,
                        g => string.Join(",", g.Select(p => $"{baseUrl}{p.ProductValue}"))
                    );
                // Retrieve vendor IDs matching the location
                var vendorIds = await _context.VendorRegistration
                    .Where(v => !string.IsNullOrEmpty(v.Area) && v.Area.ToLower().Contains(location.ToLower()))
                    .Select(v => v.id)
                    .ToListAsync();
                if (!vendorIds.Any())
                {
                    return new List<Item>(); // No vendors found matching the location
                }

                // Create SQL query with parameters
                var sql = @"
                    SELECT * 
                    FROM Itemmaster 
                    WHERE subgroupId = @SubgroupId 
                    AND VendorId IN ({0})";

                // Create SQL parameters
                var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@SubgroupId", subgroupId)
            };

                // Create the IN clause for vendor IDs
                var inClause = string.Join(", ", vendorIds.Select((_, i) => $"@VendorId{i}"));

                // Update the SQL query to include the IN clause
                sql = string.Format(sql, inClause);

                // Add parameters for vendor IDs
                sqlParameters.AddRange(vendorIds.Select((id, i) => new SqlParameter($"@VendorId{i}", id)));

                // Execute the query
                var items = await _context.Itemmaster
                    .FromSqlRaw(sql, sqlParameters.ToArray())
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var item in items)
                {
                    if (imagePaths.TryGetValue(Convert.ToInt32(item.itemid), out var path))
                    {
                        item.imagepath = path;
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                return new List<Item>();
            }
        }
        #endregion


        #region GetVendorDetailsById
        public async Task<List<VendorRegistration>> GetVendorDetailsByIds(List<int?> vendorIds)
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();

                // Step 1: Handle empty or null list gracefully
                if (vendorIds == null || !vendorIds.Any())
                {
                    return new List<VendorRegistration>();
                }

                // Step 2: Construct parameterized query
                var parameters = vendorIds
                    .Select((id, index) => new SqlParameter($"@id{index}", id ?? (object)DBNull.Value))
                    .ToArray();

                var query = @"SELECT * 
                  FROM vendorregistration 
                  WHERE id IN (" + string.Join(", ", parameters.Select(p => p.ParameterName)) + @")";

                // Step 3: Execute query and get results
                var vendors = await _context.VendorRegistration
                    .FromSqlRaw(query, parameters)
                    .AsNoTracking() // Improve performance for read-only queries
                    .ToListAsync();

                // Step 4: Modify the logo_img property using LINQ's Select method
                vendors = vendors
                    .Select(vendor =>
                    {
                        if (!string.IsNullOrEmpty(vendor.logo_img))
                        {
                            vendor.logo_img = $"{baseUrl}{vendor.logo_img}";
                        }
                        return vendor;
                    })
                    .ToList();

                return vendors;
            }
            catch (Exception ex)
            {
                return new List<VendorRegistration>();
            }
        }
        #endregion


        #region Location
        public async Task<List<VendorDetails>> GetNearbyVendorsByIdsAndLocation(List<int?> vendorIds, double currentLatitude, double currentLongitude)
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();

                // Handle empty or null list gracefully
                if (vendorIds == null || !vendorIds.Any())
                {
                    return new List<VendorDetails>();
                }

                // Construct parameterized query
                var parameters = vendorIds
                    .Select((id, index) => new SqlParameter($"@id{index}", id ?? (object)DBNull.Value))
                    .ToArray();

                var query = @"SELECT * FROM vendorregistration WHERE id IN (" + string.Join(", ", parameters.Select(p => p.ParameterName)) + @")";

                // Execute query and get results
                var vendors = await _context.VendorRegistration
                    .FromSqlRaw(query, parameters)
                    .AsNoTracking()
                    .ToListAsync();
                // Calculate distance and include vendor details
                var result = vendors
                    .Select(vendor =>
                    {
                        double? vendorLatitude = string.IsNullOrEmpty(vendor.latitude) ? (double?)null : Convert.ToDouble(vendor.latitude);
                        double? vendorLongitude = string.IsNullOrEmpty(vendor.longitude) ? (double?)null : Convert.ToDouble(vendor.longitude);

                        double distance = 0;

                        // Calculate distance only if both latitude and longitude are not null
                        if (vendorLatitude.HasValue && vendorLongitude.HasValue)
                        {
                            distance = CalculateDistance(currentLatitude, currentLongitude, vendorLatitude.Value, vendorLongitude.Value);
                        }

                        return new VendorDetails
                        {
                            Id = vendor.id,
                            VendorName = vendor.ServiceProvider,
                            Address = vendor.Address,
                            Status = vendor.Status,
                            Distance = distance > 0 ? Truncate(distance, 2) : 0 // Set distance to 0 if less than or equal to 0
                        };
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                return new List<VendorDetails>();
            }

        }
        #endregion


        #region CalculateDistance 

        //Calculate Distance between two different Latitude and Longitude
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the earth in km
            var latDistance = DegreesToRadians(lat2 - lat1);
            var lonDistance = DegreesToRadians(lon2 - lon1);
            var a =
                Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c; // Distance in km
            return distance;
        }
        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
        private double Truncate(double value, int decimalPlaces)
        {
            var multiplier = Math.Pow(10, decimalPlaces);
            return Math.Truncate(value * multiplier) / multiplier;
        }

        #endregion


        #region FetchTax
        public async Task<double> GetTaxFeesAsync()
        {
            var sqlQuery = "SELECT TOP 1 Fees FROM Tax";
            var fee = await _context.Tax
                                   .FromSqlRaw(sqlQuery)
                                   .Select(t => t.Fees)
                                   .FirstOrDefaultAsync();

            return fee.HasValue ? fee.Value : 0.0;
        }
        #endregion


        #region GetItemsBySubGroupIdwithLatitudeandLongitude
        public async Task<List<Item>> GetItemsBySubGroupIdwithLatitudeandLongitudeAsync(List<int?> subgroupIds, List<string> serviceNames, double latitude, double longitude)
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();

                // Check if the input list is null or empty
                if (subgroupIds == null || !subgroupIds.Any())
                {
                    return new List<Item>();
                }

                // Fetch packages from the database
                var packages = await _context.PackageTrans
                    .AsNoTracking()
                    .ToListAsync();

                // Aggregate image paths into a dictionary
                var imagePaths = packages
                    .Where(p => !string.IsNullOrEmpty(p.ProductValue) && p.ProductName.Contains("Image", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(p => p.TransId)
                    .ToDictionary(
                        g => g.Key,
                        g => string.Join(",", g.Select(p => $"{baseUrl}{p.ProductValue}"))
                    );

                var filteredItems = new List<Item>(); // List to hold all filtered items

                // Loop through each subgroupId and fetch items
                foreach (var subgroupId in subgroupIds)
                {
                    // Fetch items with vendors for the current subgroupId, excluding those with null subgroupId
                    var itemsWithVendors = await (from item in _context.Itemmaster
                                                  join vendor in _context.VendorRegistration
                                                  on item.vendorid equals vendor.id
                                                  where item.subgroupid == subgroupId // Check for the current subgroupId
                                                        && vendor.Status != "Pending"
                                                        && vendor.latitude != null && vendor.longitude != null
                                                  select new
                                                  {
                                                      item,
                                                      VendorLatitude = vendor.latitude,
                                                      VendorLongitude = vendor.longitude
                                                  })
                                                   .AsNoTracking()
                                                   .ToListAsync();

                    // Using Haversine formula to filter items within a 15 km radius
                    const double maxDistanceKm = 15;

                    var filteredByDistance = itemsWithVendors
                        .Where(v =>
                        {
                            if (double.TryParse(v.VendorLatitude, out double vendorLat) &&
                                double.TryParse(v.VendorLongitude, out double vendorLon))
                            {
                                return CalculateDistance(latitude, longitude, vendorLat, vendorLon) <= maxDistanceKm;
                            }
                            return false;
                        })
                        .Select(v => v.item)
                        .ToList();
                    // Apply service name filtering
                    if (serviceNames != null && serviceNames.Count > 0)
                    {
                        filteredByDistance = filteredByDistance
                        .Where(item => serviceNames.Any(serviceName => item.ItemName != null && item.ItemName.Contains(serviceName, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                    }

                    // Assign image paths to the filtered items
                    foreach (var item in filteredByDistance)
                    {
                        if (imagePaths.TryGetValue(Convert.ToInt32(item.itemid), out var path))
                        {
                            item.imagepath = path;
                        }
                        item.ServiceSeoUrl = _context.services.Where(x => x.Id == item.serviceid).FirstOrDefault().ServiceSeoUrl;
                    }

                    // Add the filtered items for the current subgroupId to the overall list
                    filteredItems.AddRange(filteredByDistance);
                }

                return filteredItems;
            }
            catch (Exception ex)
            {
                return new List<Item>();
            }
        }
        #endregion


        #region GetItemListWithDetailsByVendorId
        public async Task<List<Item>> GetItemListWithDetailsByVendorIdAsync(int vendorId)
        {
            try
            {
                var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
                var tax = _context.Tax.FirstOrDefault();
                var items = await _context.Itemmaster
                    .Where(item => item.vendorid == vendorId)
                    .AsNoTracking()
                    .ToListAsync();
                items.ForEach(item =>
                {
                    item.ItemRate = Convert.ToDouble(item.Rate);
                    item.DiscountAmount = 0;
                    item.DiscountPercent = 0;
                    item.Tax = tax.Fees;
                    item.imagepath = $"{baseUrl}{item.serviceimages}";

                });

                return items;
            }
            catch (Exception ex)
            {
                return new List<Item>();
            }

        }
        #endregion


    }
}
