using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Floppy.Infrastructure.Repositories
{
    public class SellerInfoRepository :ISellerInfoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlAddressRepositary _urlAddressRepositary;
        private readonly ILogger<SellerInfoRepository> _logger;
        public SellerInfoRepository(ApplicationDbContext context, IUrlAddressRepositary urlAddressRepositary,ILogger<SellerInfoRepository> logger)
        {
            _context = context;
            _urlAddressRepositary = urlAddressRepositary;
            _logger = logger;
        }

		#region FetchVendorRegistrationDetails
		public async Task<VendorRegistration> FetchVendorRegistrationDetails(int Id)
		{
			try
			{
				var baseUrl = await _urlAddressRepositary.GetBaseUrlAsync();
				var sql = @"SELECT * FROM VendorRegistration WHERE id = @Id";
				var parameters = new[]
				{
				new SqlParameter("@Id", Id)
			};
				var vendorRegistration = await _context.VendorRegistration
										.FromSqlRaw(sql, parameters)
										.FirstOrDefaultAsync();

				if (vendorRegistration != null)
				{
					vendorRegistration.logo_img = string.IsNullOrEmpty(vendorRegistration.logo_img)
						? null
						: $"{baseUrl}{vendorRegistration.logo_img}";
				}
				var itemIdsSql = @"SELECT * FROM Itemmaster WHERE vendorid = @VendorId";
				var itemIdsParameters = new[]
				{
					new SqlParameter("@VendorId", Id)
				};
				var itemIds = await _context.Itemmaster
									.FromSqlRaw(itemIdsSql, itemIdsParameters)
									.Select(im => im.itemid)
									.ToListAsync();
				if (itemIds.Any())
				{
					var itemIdsList = string.Join(",", itemIds);

					// Fetch Ratings from RatingReview for these Item IDs
					var ratingSql = $@"SELECT * FROM rating_review WHERE ItemId IN ({itemIdsList})";

					// Fetch ratings as strings
					var ratings = await _context.rating_review
										.FromSqlRaw(ratingSql)
										.Select(rr => rr.Rating)
										.ToListAsync();

					// Convert ratings from string to int
					var intRatings = ratings
									 .Select(r => int.TryParse(r, out var result) ? result : (int?)null)
									 .Where(r => r.HasValue)
									 .Select(r => r.Value);

					// Calculate the average rating
					var averageRating = intRatings.Any()
						? intRatings.Average()
						: (double?)null;
					var subactegoriesname = await _context.Itemmaster
										.FromSqlRaw(itemIdsSql, itemIdsParameters)
										.Select(im => im.subgroupname)
										.Distinct()
										.ToListAsync();
					vendorRegistration.AverageRating = averageRating;
					vendorRegistration.TotalServices = subactegoriesname.Count;
					vendorRegistration.servicesName = subactegoriesname;
				}
				return vendorRegistration;
			}
			catch (Exception ex) 
			{
				return new VendorRegistration();
			}
		}
		#endregion



	}

}
