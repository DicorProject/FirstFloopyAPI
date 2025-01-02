using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class LeadEntryMaster
    {
        [Column("LeadEntryId")]
        public decimal? LeadEntryId { get; set; }

        [Column("leadno")]
        public int? Leadno { get; set; }

        [Column("MaxLead")]
        public int? MaxLead { get; set; }

        [Column("leaddate")]
        public DateTime? Leaddate { get; set; }

        [Column("EntryType")]
        [StringLength(50)]
        public string? EntryType { get; set; }

        [Column("CompanyNature")]
        [StringLength(100)]
        public string? CompanyNature { get; set; }

        [Column("phoneno")]
        [StringLength(250)]
        public string? Phoneno { get; set; }

        [Column("mobileno")]
        [StringLength(250)]
        public string? Mobileno { get; set; }

        [Column("emailid")]
        public string? Emailid { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("contactperson")]
        public string? Contactperson { get; set; }

        [Column("companytype")]
        [StringLength(250)]
        public string? Companytype { get; set; }

        [Column("companyname")]
        public string? Companyname { get; set; }

        [Column("OwnerName")]
        public string? OwnerName { get; set; }

        [Column("Website")]
        public string? Website { get; set; }

        [Column("requirement")]
        public string? Requirement { get; set; }

        [Column("Compid")]
        public int? Compid { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("YearId")]
        [StringLength(50)]
        public string? YearId { get; set; }

        [Column("BranchId")]
        public int? BranchId { get; set; }

        [Column("productName")]
        [StringLength(255)]
        public string? ProductName { get; set; }

        [Column("Status")]
        [StringLength(500)]
        public string? Status { get; set; }

        [Column("source")]
        public int? Source { get; set; }

        [Column("generatedby")]
        public int? Generatedby { get; set; }

        [Column("Flag")]
        [StringLength(50)]
        public string? Flag { get; set; }

        [Column("Dealerid")]
        public int? Dealerid { get; set; }

        [Column("Priority")]
        [StringLength(50)]
        public string? Priority { get; set; }

        [Column("partyid")]
        public int? Partyid { get; set; }

        [Column("Doc")]
        public DateTime? Doc { get; set; }

        [Column("datetimestamp")]
        public DateTime? Datetimestamp { get; set; }

        [Column("contactpersonid")]
        public int? Contactpersonid { get; set; }

        [Column("shiftuserid")]
        public int? Shiftuserid { get; set; }

        [Column("handlerid")]
        public int? Handlerid { get; set; }

        [Column("QUERY_ID")]
        public string? QueryId { get; set; }

        [Column("reqdescription")]
        public string? Reqdescription { get; set; }

        [Column("eventid")]
        public int? Eventid { get; set; }

        [Column("Division")]
        public string? Division { get; set; }

        [Column("AlternateMobileno")]
        [StringLength(100)]
        public string? AlternateMobileno { get; set; }

        [Column("ExecId")]
        public int? ExecId { get; set; }

        [Column("Designation")]
        [StringLength(300)]
        public string? Designation { get; set; }

        [Column("DesignationId")]
        public int? DesignationId { get; set; }

        [Column("Country")]
        [StringLength(500)]
        public string? Country { get; set; }

        [Column("statename")]
        [StringLength(500)]
        public string? Statename { get; set; }

        [Column("cityname")]
        [StringLength(500)]
        public string? Cityname { get; set; }

        [Column("fax")]
        [StringLength(500)]
        public string? Fax { get; set; }

        [Column("AssignDate")]
        public DateTime? AssignDate { get; set; }

        [Column("leadtagid")]
        public int? Leadtagid { get; set; }

        [Column("courseid")]
        public int? Courseid { get; set; }

        [Column("Course")]
        [StringLength(200)]
        public string? Course { get; set; }

        [Column("FirstName")]
        [StringLength(500)]
        public string? FirstName { get; set; }

        [Column("LastName")]
        [StringLength(500)]
        public string? LastName { get; set; }

        [Column("Line1")]
        public string? Line1 { get; set; }

        [Column("Line2")]
        public string? Line2 { get; set; }

        [Column("Pincode")]
        [StringLength(50)]
        public string? Pincode { get; set; }

        [Column("AlternatePhoneno")]
        [StringLength(250)]
        public string? AlternatePhoneno { get; set; }

        [Column("CourseAmount")]
        public float? CourseAmount { get; set; }

        [Column("addressid")]
        public int? Addressid { get; set; }

        [Column("contactId")]
        public int? ContactId { get; set; }

        [Column("ApprovalStatus")]
        [StringLength(50)]
        public string? ApprovalStatus { get; set; }

        [Column("ApprovalRemarks")]
        [StringLength(500)]
        public string? ApprovalRemarks { get; set; }

        [Column("BaseLeadId")]
        public int? BaseLeadId { get; set; }

        [Column("ProjectId")]
        public int? ProjectId { get; set; }

        [Column("sourcename")]
        [StringLength(200)]
        public string? Sourcename { get; set; }

        [Column("gstno")]
        [StringLength(200)]
        public string? Gstno { get; set; }

        [Column("rfi_id")]
        [StringLength(1000)]
        public string? RfiId { get; set; }

        [Column("projectname")]
        [StringLength(500)]
        public string? Projectname { get; set; }

        [Column("Localdate")]
        public DateTime? Localdate { get; set; }

        [Column("HitechOwnerId")]
        public int? HitechOwnerId { get; set; }

        [Column("FileAttachment")]
        public string? FileAttachment { get; set; }

        [Column("TotalQty")]
        public double? TotalQty { get; set; }

        [Column("TotalAmt")]
        public double? TotalAmt { get; set; }

        [Column("GrandTotal")]
        public double? GrandTotal { get; set; }

        [Column("ForcastAmount")]
        public double? ForcastAmount { get; set; }

        [Column("MarketSegmentId")]
        public int? MarketSegmentId { get; set; }

        [Column("IndustryTypeId")]
        public int? IndustryTypeId { get; set; }

        [Column("TerritoryId")]
        public int? TerritoryId { get; set; }

        [Column("executiveid")]
        public int? Executiveid { get; set; }
        [Column("Slot")]
        public string? Slot { get; set; }
        [Column("PaymentMethod")]
        public string? PaymentMethod { get; set; }
        [Column("Coupon")]
        public string? Coupon { get; set; }
        [Column("PaymentrefereceOrderId")]
        public string? PaymentrefereceOrderId { get; set; }
        [Column("DeliveryStatus")]
        public string? DeliveryStatus { get; set; }
    }

    #region LeadWithCartItemsDTO
    public class LeadWithCartItemsDTO
    {
        public decimal? LeadEntryId { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? TotalAmount { get; set; }
        public string? SubgroupName { get; set; }   
        public List<CartItemDTO> Items { get; set; }
    }
    #endregion

    #region CartItemDTO
    public class CartItemDTO
    {
        public int? Id { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public double? ItemRate { get; set; }
        public double? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? UserId { get; set; }
        public string? ProcessStatus { get; set; }
        [NotMapped]
        public Double? Tax { get; set; }
        [NotMapped]
        public double? DiscountPercent { get; set; }
        [NotMapped]
        public double? DiscountAmount { get; set; }
        public string? Image { get; set; }
        public string? SubCategoryName { get; set; }
        public string? CategoryName { get; set; }
    }
    #endregion

    #region OrderModel
    public class OrderModel
    {
        public string CustomerId { get; set; }
        public string Phone { get; set; }
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }     
        public string ReturnUrl { get; set; }   
    }
    #endregion

    #region ResponseItemDTO
    public class ResponseItemDTO
    {
        public int? Id { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public double? ItemRate { get; set; }
        public double? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? UserId { get; set; }
        public string? ProcessStatus { get; set; }
        [NotMapped]
        public Double? Tax { get; set; }
        [NotMapped]
        public double? DiscountPercent { get; set; }
        [NotMapped]
        public double? DiscountAmount { get; set; }
        public string? Image { get; set; }
        public string? SubCategoryName { get; set; }
        public string? CategoryName { get; set; }
        public string? GroupType { get; set; }
        public int? VendorId { get;set; }
    }
    #endregion
}
