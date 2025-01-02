using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class Cartmaster
    {
        public int id { get; set; }

        [Column("userid")]
        public int? UserId { get; set; }

        [Column("compid")]
        public int? CompId { get; set; }

        [Column("sessionid")]
        public string? SessionId { get; set; }

        [Column("couponcode")]
        public string? CouponCode { get; set; }

        [Column("couponamount")]
        public double? CouponAmount { get; set; }

        [Column("itemid")]
        public int? ItemId { get; set; }

        [Column("itemname")]
        public string? ItemName { get; set; }

        [Column("itemrate")]
        public double? ItemRate { get; set; }

        [Column("quantity")]
        public double? Quantity { get; set; }

        [Column("total")]
        public double? Total { get; set; }

        [Column("subtotal")]
        public double? Subtotal { get; set; }

        [Column("shippingfee")]
        public double? ShippingFee { get; set; }

        [Column("giftwrap")]
        public double? GiftWrap { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("grandtotal")]
        public double? GrandTotal { get; set; }

        [Column("carttype")]
        public string? CartType { get; set; }

        [Column("stockstatus")]
        public string? StockStatus { get; set; }

        [Column("processstatus")]
        public string? ProcessStatus { get; set; }

        [Column("orderid")]
        public string? OrderId { get; set; }

        [Column("gstpercent")]
        public double? GstPercent { get; set; }

        [Column("discountpercent")]
        public double? DiscountPercent { get; set; }

        [Column("discountamount")]
        public double? DiscountAmount { get; set; }

        [Column("cdpercent")]
        public double? CdPercent { get; set; }

        [Column("cdamount")]
        public double? CdAmount { get; set; }

        [Column("gstamount")]
        public double? GstAmount { get; set; }

        [Column("convertunitid")]
        public int? ConvertUnitId { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }
        [Column("Image")]
        public string? Image {  get; set; } 
    }

}
