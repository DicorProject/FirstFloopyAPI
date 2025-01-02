namespace Floppy.Domain.Entities
{
    public class CouponMaster
    {
        public int CouponId { get; set; }
        public string CouponDescription { get; set; }
        public decimal Percentage { get; set; }
        public decimal? Amount { get; set; }
        public int? UserId { get; set; }
        public int? Status { get; set; }
        public string Currency { get; set; }
        public string? CoupanCode { get; set; }
        public DateTime? Validityfrom { get; set; }
        public DateTime? Validityto { get; set; }
        public int? VoucherType { get; set; }
        public int? Compid { get; set; }
        public int? ActivationStatus { get; set; }
        public int? ShowonDashboard { get; set; }   

	}

    #region DiscountResultResponseModel
    public class DiscountResult
    {
        public decimal DiscountedPrice { get; set; }
        public decimal DiscountAmount { get; set; }
    }
    #endregion
}
