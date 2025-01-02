namespace Floppy.Domain.Entities
{
    public class LeadTrans
    {
        public int Leadtransid { get; set; }
        public int? Leadno { get; set; }
        public int? ItemId { get; set; }
        public double? Quantity { get; set; }
        public int? LeadEntryId { get; set; }
        public int? OItemId { get; set; }
        public double? SalesPrice { get; set; }
        public int? Compid { get; set; }
        public int? Userid { get; set; }
        public string? Yearid { get; set; } 
        public int? Branchid { get; set; }
        public string? ItemSpecification { get; set; }  
        public string? Imagepath { get; set; }  
        public int? MergeFlag { get; set; }
        public double? MRP { get; set; }
        public double? Amount { get; set; }
        public double? Discount { get; set; }
        public int? UnitId { get; set; }
        public int? ItembrandId { get; set; }
    }

    #region CartMasterModel
    public class CartMasterModel
    {
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Price { get; set; }
        public string? ItemName { get; set; }
        public double? Quantity { get; set; }
        public string? ProcessStatus { get; set; }
        public string? Image { get; set; }
    }
    #endregion

    #region LeadEntryDetailsModel
    public class LeadEntryDetailsModel
    {
        public decimal? OrderId { get; set; }
        public int? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? EmailId { get; set; }
        public string? PhoneNo { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public string? Pincode { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Country { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
    }
    #endregion

    #region LeadByProductModel
    public class LeadByProductModel
    {
        public CartMasterModel Cart { get; set; }
        public LeadEntryDetailsModel LeadEntryDetails { get; set; }
    }
    #endregion
}
