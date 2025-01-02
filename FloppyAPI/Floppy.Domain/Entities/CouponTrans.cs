namespace Floppy.Domain.Entities
{
    public class CouponTrans
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public int? CouponId { get; set; }
        public DateTime? CouponUsedDate { get; set; }
    }

    #region couponResponseModel
    public class coupon
    {
        public int? UserId { get; set; }
        public int? CouponId { get; set; }
        public decimal TotalPrice {get; set;}
        public string CouponCode { get; set;}   
    }
    #endregion
}
