using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using MailKit.Search;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Floppy.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartRepository> _logger;       
        public CartRepository(ApplicationDbContext context,ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
		#region AddCartItems
		public bool AddCart(decimal entryId, int userId, double totalAmt, float totalQty,
					string firstName, string lastName, string address, string emailId, string phoneNo,
					string stateName, string cityName, string pincode, DateTime dateTimestamp,
					string country, int compId, List<int> itemIds, string slot, string coupon, string latitude, string longitude)
		{
			var success = false;

			// Define extra parameters
			var leadDate = DateTime.Now;
			var leadForm = "Website";
			var status = "Pending";
			var yearId = "2024-25";



			try
			{
				var itemIdsString = string.Join(",", itemIds); // Converts list to comma-separated string

				var result = _context.Database.ExecuteSqlRaw("EXEC Add_ToCart @TotalAmt, @TotalQty, @FirstName, @LastName, @Address, @EmailId, @MobileNo, " +
																				"@Location, @LeadDate, @BranchID, @YearId, @Status, @StateName, @CityName, @Pincode, @DateTimeStamp, @Country, " +
																				"@CompId, @UserId, @Slot, @Coupon, @LeadEntryFrom, @Latitude, @Longitude, @ItemIds",
					new SqlParameter("@TotalAmt", totalAmt),
					new SqlParameter("@TotalQty", totalQty),
					new SqlParameter("@FirstName", (object)firstName ?? DBNull.Value),
					new SqlParameter("@LastName", (object)lastName ?? DBNull.Value),
					new SqlParameter("@Address", (object)address ?? DBNull.Value),
					new SqlParameter("@EmailId", (object)emailId ?? DBNull.Value),
					new SqlParameter("@MobileNo", (object)phoneNo ?? DBNull.Value),
					new SqlParameter("@Location", (object)cityName ?? DBNull.Value),
					new SqlParameter("@LeadDate", leadDate),
					new SqlParameter("@BranchID", compId),
					new SqlParameter("@YearId", yearId),
					new SqlParameter("@Status", status),
					new SqlParameter("@StateName", (object)stateName ?? DBNull.Value),
					new SqlParameter("@CityName", (object)cityName ?? DBNull.Value),
					new SqlParameter("@Pincode", (object)pincode ?? DBNull.Value),
					new SqlParameter("@DateTimeStamp", dateTimestamp),
					new SqlParameter("@Country", (object)country ?? DBNull.Value),
					new SqlParameter("@CompId", compId),
					new SqlParameter("@UserId", userId),
					new SqlParameter("@Slot", (object)slot ?? DBNull.Value),
					new SqlParameter("@Coupon", (object)coupon ?? DBNull.Value),
					new SqlParameter("@LeadEntryFrom", leadForm),
					new SqlParameter("@Latitude", latitude),
					new SqlParameter("@Longitude", longitude),
					new SqlParameter("@ItemIds", itemIdsString)
				);

				if (result > 0)
				{
					success = true;
				}
			}
			catch (SqlException ex)
			{
				Console.WriteLine($"SQL Exception: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
			}
			return success;
		}
		#endregion

		#region UpdateCartItem
		public async Task<bool> UpdateCart(decimal entryId, int userId, string productName, float totalAmt, float totalQty,
                                    string firstName, string address, string emailid, string phoneno,
                                    string statename, string cityname, string pincode, DateTime datetimestam,
                                    string country, int compid)
        {
            var sql = @"
        UPDATE LeadEntryMaster
        SET 
            productName = @productName,
            TotalAmt = @TotalAmt,
            TotalQty = @TotalQty,
            FirstName = @FirstName,
            address = @address,
            emailid = @emailid,
            phoneno = @phoneno,
            statename = @statename,
            cityname = @cityname,
            Pincode = @Pincode,
            datetimestamp = @datetimestam,
            Country = @Country,
            Compid = @Compid
        WHERE 
            LeadEntryId = @LeadEntryId AND UserId = @UserId";

            var result = await _context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@LeadEntryId", entryId),
                new SqlParameter("@productName", (object)productName ?? DBNull.Value),
                new SqlParameter("@TotalAmt", totalAmt),
                new SqlParameter("@TotalQty", totalQty),
                new SqlParameter("@FirstName", (object)firstName ?? DBNull.Value),
                new SqlParameter("@address", (object)address ?? DBNull.Value),
                new SqlParameter("@emailid", (object)emailid ?? DBNull.Value),
                new SqlParameter("@phoneno", (object)phoneno ?? DBNull.Value),
                new SqlParameter("@statename", (object)statename ?? DBNull.Value),
                new SqlParameter("@cityname", (object)cityname ?? DBNull.Value),
                new SqlParameter("@Pincode", (object)pincode ?? DBNull.Value),
                new SqlParameter("@datetimestam", datetimestam),
                new SqlParameter("@Country", (object)country ?? DBNull.Value),
                new SqlParameter("@Compid", compid),
                new SqlParameter("@UserId", userId)
            );

            return result > 0;
        }
        #endregion

        #region GetLeadsByUserId
        //public async Task<List<LeadWithCartItemsDTO>> GetLeadsByUserIdAsync(int userId)
        //{
        //    // Query to get lead entries for the specified user
        //    var leadEntriesSql = @"SELECT * FROM LeadEntryMaster WHERE UserId = @UserId ORDER BY LeadEntryId DESC";
        //    var leadEntries = await _context.LeadEntryMaster
        //        .FromSqlRaw(leadEntriesSql, new SqlParameter("@UserId", userId))
        //        .ToListAsync();

        //    // Query to get cart items where the order ID matches any lead entry ID for the specified user
        //    var cartItemsSql = @"SELECT * FROM Cartmaster WHERE OrderId IN (SELECT [LeadEntryId] FROM LeadEntryMaster WHERE UserId = @UserId) AND UserId = @UserId";
        //    var cartItems = await _context.cartmaster
        //        .FromSqlRaw(cartItemsSql, new SqlParameter("@UserId", userId))
        //        .ToListAsync();

        //    // Construct the result by combining lead entries and their corresponding cart items
        //    var result = leadEntries.Select(entry => new LeadWithCartItemsDTO
        //    {
        //        LeadEntryId = entry.LeadEntryId,
        //        UserId = userId,
        //        Date = entry.Datetimestamp?.Date.ToString("d MMM yyyy"),
        //        Time = entry.Slot,
        //        PaymentMethod = entry.PaymentMethod,
        //        PaymentStatus = entry.Status,
        //        DeliveryStatus= entry.DeliveryStatus,
        //        TotalAmount=entry.TotalAmt.ToString(),
        //        Items = cartItems.Where(cart =>Convert.ToDecimal(cart.OrderId)== entry.LeadEntryId && cart.UserId == userId).Select(cart => new CartItemDTO
        //        {
        //            Id = cart.id,
        //            ItemId = cart.ItemId,
        //            ItemName = cart.ItemName,
        //            ItemRate = cart.ItemRate,
        //            Quantity = cart.Quantity,
        //            Price = cart.Price,
        //            UserId=cart.UserId, 
        //            ProcessStatus= cart.ProcessStatus,
        //            Image=cart.Image, 
        //            SubCategoryName=_context.Itemmaster.Where(x=>x.itemid== cart.ItemId).FirstOrDefaultAsync().Result.subgroupname,
        //            CategoryName= _context.Itemmaster.Where(x => x.itemid == cart.ItemId).FirstOrDefaultAsync().Result.maingroupname

        //        }).ToList()
        //    }).ToList();

        //    return result;
        //}

        public async Task<List<LeadWithCartItemsDTO>> GetLeadsByUserIdAsync(int userId)
        {

            var leadEntriesSql = @"SELECT * FROM LeadEntryMaster WHERE UserId = @UserId and PaymentrefereceOrderId IS NOT NULL  ORDER BY LeadEntryId DESC";
            var leadEntries = await _context.LeadEntryMaster
                .FromSqlRaw(leadEntriesSql, new SqlParameter("@UserId", userId))
                .ToListAsync();

            // If no lead entries are found, return an empty list
            if (leadEntries == null || !leadEntries.Any())
                return new List<LeadWithCartItemsDTO>();
            var cartItemsSql = @"SELECT* FROM Cartmaster WHERE OrderId IN(SELECT[LeadEntryId] FROM LeadEntryMaster WHERE UserId = @UserId) AND UserId = @UserId";

            var cartItems = await _context.cartmaster
                .FromSqlRaw(cartItemsSql, new SqlParameter("@UserId", userId))
                .ToListAsync();
            var result = leadEntries
                .Select(entry => new LeadWithCartItemsDTO
                {
                    LeadEntryId = entry.LeadEntryId,
                    UserId = userId,
                    Date = entry.Datetimestamp?.Date.ToString("d MMM yyyy"),
                    Time = entry.Slot,
                    PaymentMethod = entry.PaymentMethod,
                    PaymentStatus = entry.Status,
                    DeliveryStatus = entry.DeliveryStatus,
                    TotalAmount = entry.TotalAmt.ToString(),
                    // Fetch cart items for the current lead
                    Items = cartItems
                        .Where(cart => cart.OrderId == entry.LeadEntryId.ToString() && cart.UserId == userId)
                        .Select(cart => new CartItemDTO
                        {
                            Id = cart.id,
                            ItemId = cart.ItemId,
                            ItemName = cart.ItemName,
                            ItemRate = cart.ItemRate,
                            Quantity = cart.Quantity,
                            Price = cart.Price,
                            UserId = cart.UserId,
                            ProcessStatus = cart.ProcessStatus,
                            Image = cart.Image,
                            SubCategoryName = _context.Itemmaster.Where(x => x.itemid == cart.ItemId).FirstOrDefaultAsync().Result?.subgroupname,
                            CategoryName = _context.Itemmaster.Where(x => x.itemid == cart.ItemId).FirstOrDefaultAsync().Result?.maingroupname
                        })
                        .ToList()
                })
                // Only return lead entries that have non-empty cart items
                .Where(entry => entry.Items != null && entry.Items.Any())
                .ToList();

            return result;
        }
        #endregion

        #region GetMaxLeadEntryId
        public async Task<int> GetMaxLeadEntryIdAsync()
        {
            try
            {
                var maxLeadEntryId = await _context.LeadEntryMaster
                    .MaxAsync(l => l.LeadEntryId);

                return (int)maxLeadEntryId;
            }
            catch (Exception ex)
            {
				// Optionally log the exception
				Console.WriteLine(ex.Message);
				return 0;
            }
        }
        #endregion

        #region GetLeadByProductId
        public async Task<LeadByProductModel> GetLeadByProductIdAsync(int productId)
        {
            var cartMasterSql = @"SELECT * FROM cartmaster WHERE id = @ProductId";

            var cartMaster = await _context.cartmaster
                .FromSqlRaw(cartMasterSql, new SqlParameter("@ProductId", productId))
                .FirstOrDefaultAsync();

            if (cartMaster == null)
            {
                return null;
            }

            var leadEntrySql = @"SELECT * FROM LeadEntryMaster WHERE LeadEntryId = @LeadEntryId";

            var leadEntry = await _context.LeadEntryMaster
                .FromSqlRaw(leadEntrySql, new SqlParameter("@LeadEntryId", cartMaster.OrderId))
                .FirstOrDefaultAsync();

            var result = new LeadByProductModel
            {
                Cart = new CartMasterModel
                {
                    CartId = cartMaster.id,
                    ProductId = cartMaster.ItemId,
                    Price = cartMaster.Price,
                    ItemName = cartMaster.ItemName,
                    Quantity = cartMaster.Quantity,
                    ProcessStatus = cartMaster.ProcessStatus,
                    Image= cartMaster.Image,    
                    
                },
                LeadEntryDetails = leadEntry == null ? null : new LeadEntryDetailsModel
                {
                    OrderId = leadEntry.LeadEntryId,
                    UserId = leadEntry.UserId,
                    FirstName = leadEntry.FirstName,
                    LastName = leadEntry.LastName,
                    Address = leadEntry.Address,
                    EmailId = leadEntry.Emailid,
                    PhoneNo = leadEntry.Phoneno,
                    StateName = leadEntry.Statename,
                    CityName = leadEntry.Cityname,
                    Pincode = leadEntry.Pincode,
                    Date = leadEntry.Datetimestamp?.Date.ToString("d MMM yyyy"),
                    Time = leadEntry.Slot,
                    Country = leadEntry.Country,
                    PaymentMethod = leadEntry.PaymentMethod,
                    PaymentStatus = leadEntry.Status
                }
            };

            return result;
        }
        #endregion

        #region GetOrderDetailsByOrderId
        public async Task<LeadByProductModel> GetOrderDetailsByOrderIdAsync(int productId)
        {
            var cartMasterSql = @"SELECT * FROM cartmaster WHERE orderid = @ProductId";

            var cartMaster = await _context.cartmaster
                .FromSqlRaw(cartMasterSql, new SqlParameter("@ProductId", productId))
                .FirstOrDefaultAsync();

            if (cartMaster == null)
            {
                return null;
            }

            var leadEntrySql = @"SELECT * FROM LeadEntryMaster WHERE LeadEntryId = @LeadEntryId";

            var leadEntry = await _context.LeadEntryMaster
                .FromSqlRaw(leadEntrySql, new SqlParameter("@LeadEntryId", cartMaster.OrderId))
                .FirstOrDefaultAsync();

            var result = new LeadByProductModel
            {
                Cart = new CartMasterModel
                {
                    CartId = cartMaster.id,
                    ProductId = cartMaster.ItemId,
                    Price = cartMaster.Price,
                    ItemName = cartMaster.ItemName,
                    Quantity = cartMaster.Quantity,
                    ProcessStatus = cartMaster.ProcessStatus,
                    Image = cartMaster.Image,

                },
                LeadEntryDetails = leadEntry == null ? null : new LeadEntryDetailsModel
                {
                    OrderId = leadEntry.LeadEntryId,
                    UserId = leadEntry.UserId,
                    FirstName = leadEntry.FirstName,
                    LastName = leadEntry.LastName,
                    Address = leadEntry.Address,
                    EmailId = leadEntry.Emailid,
                    PhoneNo = leadEntry.Phoneno,
                    StateName = leadEntry.Statename,
                    CityName = leadEntry.Cityname,
                    Pincode = leadEntry.Pincode,
                    Date = leadEntry.Datetimestamp?.Date.ToString("d MMM yyyy"),
                    Time = leadEntry.Slot,
                    Country = leadEntry.Country,
                    PaymentMethod = leadEntry.PaymentMethod,
                    PaymentStatus = leadEntry.Status
                }
            };

            return result;
        }
        #endregion

        #region GetCartItemListsByUserId
        public async Task<List<ResponseItemDTO>> GetByIdAsync(int userId)
        {
            // Fetch the tax value
            var sqlQuery = "SELECT TOP 1 Fees FROM Tax";
            var fee = await _context.Tax
                                   .FromSqlRaw(sqlQuery)
                                   .Select(t => t.Fees)
                                   .FirstOrDefaultAsync();

            // Fetch cart items for the specified user
            var query = @"SELECT * FROM cartmaster WHERE userid = @Id AND orderid IS NULL ORDER BY id DESC";

            var cartItems = await _context.cartmaster
                .FromSqlRaw(query, new SqlParameter("@Id", userId))
                .ToListAsync();

            // Group items by ItemId and calculate aggregates
            var groupedItems = cartItems
                .GroupBy(c => new { c.ItemId, c.ItemName, c.ItemRate, c.Image })
                .Select(g => new
                {
                    Id = g.First().id,  // Use the Id from the first item in the group
                    UserId = g.First().UserId,
                    ItemId = g.Key.ItemId,
                    ItemName = g.Key.ItemName,
                    ItemRate = g.Key.ItemRate,
                    Quantity = g.Sum(c => c.Quantity), // Sum of quantities
                    Price = g.Sum(c => c.Price), // Sum of prices
                    ProcessStatus = g.First().ProcessStatus, // Use the ProcessStatus from the first item
                    Image = g.Key.Image,
                    Ids = g.Select(item => item.id).ToList(),
                })
                .ToList();

            List<int> idsToRemoveFromCart = groupedItems
            .SelectMany(g => g.Ids)
             .Where(id => !groupedItems.Select(g => g.Id).Contains(id)) // Exclude IDs that match groupedItems.Id
             .ToList();



            if (idsToRemoveFromCart.Any())
            {
                // Create a comma-separated string of IDs
                string idsToRemove = string.Join(",", idsToRemoveFromCart);

                // deleted the duplicate data which we included in group by
                var deleteQuery = $@"DELETE FROM cartmaster WHERE id IN ({idsToRemove}) AND userid = @UserId AND orderid IS NULL;";
                await _context.Database.ExecuteSqlRawAsync(deleteQuery,
                new SqlParameter("@UserId", userId));
            }

            // Update price and quantity for the retained items
            foreach (var item in groupedItems)
            {
                var updateQuery = @"
                        UPDATE cartmaster
                        SET Quantity = @Quantity, Price = @Price
                        WHERE id = @Id";

                await _context.Database.ExecuteSqlRawAsync(updateQuery,
                    new SqlParameter("@Quantity", item.Quantity),
                    new SqlParameter("@Price", item.Price),
                    new SqlParameter("@Id", item.Id));
            }

            // Fetch the updated cart items
            var updatedQuery = @"SELECT * FROM cartmaster  WHERE userid = @Id AND orderid IS NULL";

            var updatedCartItems = await _context.cartmaster
                .FromSqlRaw(updatedQuery, new SqlParameter("@Id", userId))
                .Select(c => new ResponseItemDTO
                {
                    Id = c.id,
                    UserId = c.UserId,
                    ItemId = c.ItemId,
                    ItemName = c.ItemName,
                    ItemRate = c.ItemRate,
                    Quantity = c.Quantity,
                    Price = c.Price,
                    ProcessStatus = c.ProcessStatus,
                    Tax = fee,
                    DiscountAmount = 0,
                    DiscountPercent = 0,
                    Image = c.Image,
                    VendorId = _context.Itemmaster.Where(x => x.itemid == c.ItemId).Select(x => x.vendorid).FirstOrDefault(),
                    SubCategoryName = _context.Itemmaster.Where(x => x.itemid == c.ItemId).Select(x => x.subgroupname).FirstOrDefault(),
                    CategoryName = _context.Itemmaster.Where(x => x.itemid == c.ItemId).Select(x => x.maingroupname).FirstOrDefault(),
                    GroupType = _context.ClassificationMaster.Where(cm => cm.MainId == _context.Itemmaster.Where(im => im.itemid == c.ItemId).Select(im => im.maingroupid).FirstOrDefault()).Select(cm => cm.GroupType).FirstOrDefault()}).ToListAsync();

            return updatedCartItems;
        }
        #endregion

        #region AddCartItems
        public async Task<bool> AddAsync(List<CartItemDTO> cartMasters)
        {
            foreach (var cartMaster in cartMasters)
            {
                var query = @"INSERT INTO cartmaster (userid, itemid, itemname, itemrate, quantity, processstatus, price,Image,compid)
                      VALUES (@UserId, @ItemId, @ItemName, @ItemRate, @Quantity, @ProcessStatus, @Price,@Image,@CompId);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);";

                try
                {
                    var result = await _context.Database.ExecuteSqlRawAsync(query,
                        new SqlParameter("@UserId", cartMaster.UserId),
                        new SqlParameter("@ItemId", cartMaster.ItemId),
                        new SqlParameter("@ItemName", cartMaster.ItemName),
                        new SqlParameter("@ItemRate", cartMaster.ItemRate),
                        new SqlParameter("@Quantity", cartMaster.Quantity),
                        new SqlParameter("@ProcessStatus", cartMaster.ProcessStatus),
                        new SqlParameter("@Price", cartMaster.Price),
                        new SqlParameter("@CompId",1),
                        new SqlParameter("@Image",cartMaster.Image)
                    );

                    // Optionally check if the result is a valid ID
                    if (result <= 0)
                    {
                        return false; // Indicates failure in inserting the record
                    }
                }
                catch
                {
                    return false; // Indicates an exception occurred during the insert
                }
            }

            return true; // All inserts were successful
        }
        #endregion

        #region UpdateCartItems
        public async Task<bool> UpdateAsync(List<CartItemDTO> cartMasterDTOs)
        {
            try
            {
                var ids = cartMasterDTOs.Select(dto => dto.Id).ToList();

                foreach (var id in ids)
                {
                    var queryCheckId = $@"SELECT COUNT(*) FROM cartmaster WHERE id = {id}";

                    // Execute the query
                    var existingIdCount = await _context.Database
                        .ExecuteSqlRawAsync(queryCheckId); // Executes the query and returns the number of affected rows

                    if (existingIdCount == 0)
                    {
                        return false; // ID not present in the table
                    }
                }
                // Step 2: Update the records
                foreach (var cartMasterDTO in cartMasterDTOs)
                {
                    var query = @"UPDATE cartmaster
                            SET itemname = @ItemName, itemrate = @ItemRate, quantity = @Quantity, 
                            userid = @UserId, itemid = @ItemId,price=@Price ,Image=@Image,processstatus = @ProcessStatus
                            WHERE id = @Id";

                    var affectedRows = await _context.Database.ExecuteSqlRawAsync(query,
                        new SqlParameter("@Id", cartMasterDTO.Id),
                        new SqlParameter("@ItemName", (object)cartMasterDTO.ItemName ?? DBNull.Value),
                        new SqlParameter("@ItemRate", (object)cartMasterDTO.ItemRate ?? DBNull.Value),
                        new SqlParameter("@Quantity", (object)cartMasterDTO.Quantity ?? DBNull.Value),
                        new SqlParameter("@UserId", (object)cartMasterDTO.UserId ?? DBNull.Value),
                        new SqlParameter("@ItemId", (object)cartMasterDTO.ItemId ?? DBNull.Value),
                        new SqlParameter("@ProcessStatus", (object)cartMasterDTO.ProcessStatus ?? DBNull.Value),
                        new SqlParameter("@Price", (object)cartMasterDTO.Price ?? DBNull.Value),
                        new SqlParameter("@Image", (object)cartMasterDTO.Image?? DBNull.Value)
                    );
                    if (affectedRows == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteCartItems
        public async Task<bool> DeleteAsync(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return false;
            }

            var query = $"DELETE FROM cartmaster WHERE id IN ({string.Join(",", ids)})";
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(query);

            return affectedRows > 0;
        }
        #endregion

        #region UpdatePaymentData
        public async Task<bool> UpdatePaymentData(int userId, string paymentMethod, string status,string orderId)
        {
            var success = false;

            var updateSql = @"UPDATE LeadEntryMaster SET PaymentMethod = @PaymentMethod,Status = @Status,PaymentrefereceOrderId=@OrderId,DeliveryStatus=@deliveryStatus WHERE UserId = @UserId AND PaymentrefereceOrderId IS NULL";

            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(updateSql,
                    new SqlParameter("@PaymentMethod", (object)paymentMethod ?? DBNull.Value),
                    new SqlParameter("@Status", (object)status ?? DBNull.Value),
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@OrderId", (object)orderId ?? DBNull.Value),
                    new SqlParameter("@deliveryStatus","Pending")
                );

                success = result > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
            }

            return success;
        }
        #endregion

        #region GetLeadEntryId
        public async Task<int> GetLeadEntryId(string orderId, int userId)
        {
            // Retrieve the LeadEntryId, handling possible nulls
            var leadEntryId = await _context.LeadEntryMaster
                .Where(l => l.UserId == userId && l.LeadEntryId.ToString() == orderId)
                .Select(l => (int?)l.LeadEntryId)  // Use nullable int
                .FirstOrDefaultAsync();

            // Return 0 or an appropriate default value if no matching entry is found
            return leadEntryId ?? 0;
        }
        #endregion

        #region DeleteLeadEntry
        public async Task<bool> CancelLeadEntryAsync(int leadEntryId)
        {
            bool isUpdated = false;

            try
            {
                var updateLeadEntrySql = @"UPDATE LeadEntryMaster SET DeliveryStatus = 'Cancelled' WHERE LeadEntryId = @LeadEntryId";
                var leadEntryIdParameter = new SqlParameter("@LeadEntryId", leadEntryId);
                var leadEntryUpdated = await _context.Database.ExecuteSqlRawAsync(updateLeadEntrySql, leadEntryIdParameter);
                isUpdated = leadEntryUpdated > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the lead entry: {ex.Message}", ex);
            }

            return isUpdated;
        }
        #endregion

        #region getEmailfromLeadentrymasterByLeadId
        public string GetEmailFromLeadEntrymasterByLeadId(int leadId)
        {
            // Find the lead entry by its ID
            var leadEntry = _context.LeadEntryMaster
                .FirstOrDefault(x => x.LeadEntryId == leadId);
            return leadEntry?.Emailid; 
        }

		#endregion

		#region getTotalAmountfromLeadentrymasterByLeadId
		public string GetTotalAmountFromLeadEntrymasterByLeadId(int leadId)
		{
			// Find the lead entry by its ID
			var leadEntry = _context.LeadEntryMaster
				.FirstOrDefault(x => x.LeadEntryId == leadId);
			return leadEntry?.TotalAmt.ToString();
		}

		#endregion

		#region UpdateLeadEntryAsync
		public async Task<bool> UpdateLeadEntryAsync(int leadEntryId, string newSlot, DateTime newDateTimestamp)
        {
            // Define the raw SQL update command
            var updateSql = @"UPDATE LeadEntryMaster
                       SET Slot = @Slot, Datetimestamp = @DateTimestamp
                       WHERE LeadEntryId = @LeadEntryId";

            // Create SQL parameters to avoid SQL injection
            var parameters = new[]
            {
                    new SqlParameter("@Slot", newSlot),
                    new SqlParameter("@DateTimestamp", newDateTimestamp),
                    new SqlParameter("@LeadEntryId", leadEntryId)
            };

            // Execute the SQL command
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);

            // Return true if one or more rows were updated, otherwise false
            return affectedRows > 0;
        }
        #endregion

        #region CheckAndAddToCartAsync
        public async Task<bool> CheckAndAddToCartAsync(int? itemId, int? userId)
        {
            // Get the cart items for the given user
            var cartItemDetails = await GetByIdAsync(userId.Value);
            if (cartItemDetails == null || !cartItemDetails.Any())
            {
                return true;
            }
            var vendorDetails = _context.Itemmaster.FirstOrDefault(x => x.itemid == itemId);
            bool exists = cartItemDetails.Any(cartItem =>
                _context.Itemmaster.Any(item =>
                    item.itemid == cartItem.ItemId &&
                    item.vendorid == vendorDetails.vendorid &&
                    item.maingroupid == vendorDetails.maingroupid));

            return exists;
        }
        #endregion

        #region GetVendorIdIfAllItemsSameVendor
        public int? GetVendorIdIfAllItemsSameVendor(List<decimal?> itemIds)
        {
            try
            {

                var itemIdsString = string.Join(",", itemIds.Select(x => x.HasValue ? x.Value.ToString() : "NULL"));

                // Use the SQL IN clause with a parameterized query
                var query = $"SELECT * FROM Itemmaster WHERE itemid IN ({itemIdsString})";

                var items = _context.Itemmaster.FromSqlRaw(query).ToList();

                if (!items.Any())
                    return null;

                var vendorId = items.First().vendorid;
                bool allSameVendor = items.All(x => x.vendorid == vendorId);

                return allSameVendor ? vendorId : null;
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
				return 0;
            }
        }
        #endregion

        #region GetVendorContactDetailsById
        public object GetVendorContactDetailsById(int vendorId)
        {
            try
            {
                // Get the vendor's email and mobile number first
                var vendorDetails = _context.VendorRegistration
                                            .Where(v => v.id == vendorId)
                                            .Select(v => new
                                            {
                                                Email = v.Email,
                                                MobileNumber = v.ContactNo
                                            })
                                            .FirstOrDefault();

                if (vendorDetails == null)
                {
                    return null; // Vendor not found
                }

                // Use the vendor's mobile number to get the device token from tb_login
                var loginDetails = _context.tb_login
                                           .Where(l => l.RegistrationId == vendorId)
                                           .Select(l => new
                                           {
                                               DeviceToken = l.Token
                                           })
                                           .FirstOrDefault();

                // Return the combined result
                return new
                {
                    vendorDetails.Email,
                    vendorDetails.MobileNumber,
                    DeviceToken = loginDetails?.DeviceToken
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Error = ex.Message.ToString(),
                };
            }
        }


        #endregion

        #region SaveDatainLeadvendorTrans
        public async Task<bool> SaveDatainLeadvendorTrans(int vendorId, int userId,int leadId)
        {
            string insertSql = @"
                    INSERT INTO [Firstfloppywebsiteandapp].[dbo].[LeadVandorTrans]
                    ([Vendorid], [Leadid], [Compid], [Branchid], [Userid], [Yearid])
                    VALUES
                    (@VendorId, @LeadId, @CompId, @BranchId, @UserId, @YearId);";

            // Execute the SQL command with parameters
            var result = await _context.Database.ExecuteSqlRawAsync(insertSql,
                new SqlParameter("@VendorId", vendorId),
                new SqlParameter("@LeadId", leadId),
                new SqlParameter("@CompId", 1),
                new SqlParameter("@BranchId",1),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@YearId", "2024-25")
            );
            return result > 0;
        }
        #endregion

        #region GetLeadVandorTransDetails
        public int? GetLeadVendorTransDetails(int leadId)
        {
            var vendorId = _context.LeadVandorTrans
                .Where(l => l.LeadId == leadId)
                .Select(l => (int?)l.VendorId)
                .FirstOrDefault(); 
            return vendorId;
        }
        #endregion

    }
}
