using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface ICartRepository
    {
        #region AddCart&updatecart
        bool AddCart(decimal entryId, int UserId, double TotalAmt, float TotalQty,
                                        string FirstName, string LastName, string address, string emailid, string phoneno,
                                        string statename, string cityname, string Pincode, DateTime datetimestam,
                                        string Country, int Compid, List<int> leadTransList, string Slot, string Coupon, string latitude, string longitude);
        Task<bool> UpdateCart(decimal entryId, int userId, string productName, float totalAmt, float totalQty,
                                    string firstName, string address, string emailid, string phoneno,
                                    string statename, string cityname, string pincode, DateTime datetimestam,
                                    string country, int compid);
        #endregion
        Task<List<LeadWithCartItemsDTO>> GetLeadsByUserIdAsync(int userId);
        Task<int> GetMaxLeadEntryIdAsync();
        Task<LeadByProductModel> GetLeadByProductIdAsync(int productId);
        Task<List<ResponseItemDTO>> GetByIdAsync(int userId);
        Task<bool> AddAsync(List<CartItemDTO> cartMasters);
        Task<bool> UpdateAsync(List<CartItemDTO> cartMasterDTOs);
        Task<bool> DeleteAsync(List<int> ids);
        Task<bool> UpdatePaymentData(int userId, string paymentMethod, string status, string orderId);
        Task<int> GetLeadEntryId(string orderId, int userId);
        Task<bool> CancelLeadEntryAsync(int leadEntryId);
        Task<bool> UpdateLeadEntryAsync(int leadEntryId, string newSlot, DateTime newDateTimestamp);
        Task<bool> CheckAndAddToCartAsync(int? itemId, int? userId);
        int? GetVendorIdIfAllItemsSameVendor(List<decimal?> itemIds);
        object GetVendorContactDetailsById(int vendorId);
        Task<bool> SaveDatainLeadvendorTrans(int vendorId, int userId, int leadId);
        int? GetLeadVendorTransDetails(int leadId);
        Task<LeadByProductModel> GetOrderDetailsByOrderIdAsync(int productId);
        string GetEmailFromLeadEntrymasterByLeadId(int leadId);
        string GetTotalAmountFromLeadEntrymasterByLeadId(int leadId);

	}
}
