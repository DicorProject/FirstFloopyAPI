using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class Item
    {
        //public decimal DateTimesTampNo { get; set; }

        //[StringLength(800)]
        //public string? ItemCode { get; set; }

        //public string? ItemName { get; set; }

        //public int? ParentItem { get; set; }

        //public int? UnitID { get; set; }

        //public float? MRP { get; set; }

        //public float? PurchaseRate { get; set; }

        //public string? BarCode { get; set; }

        //public float? VatPercent { get; set; }

        //public int? CompanyID { get; set; }

        //public int? BranchId { get; set; }

        //public int? UserID { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime? EntryDate { get; set; } = DateTime.Now; // Default to current date

        //[StringLength(50)]
        //public string? ItemType { get; set; }

        //[StringLength(50)]
        //public string? ItemType2 { get; set; }

        //public float? OpeningStock { get; set; }

        //public float? VatValue { get; set; }

        //[StringLength(50)]
        //public string? VatFlag { get; set; }

        //public int? ClosingStock { get; set; }

        //public int? ReOrderLevel { get; set; }

        //public int? MinimumStock { get; set; }

        //[StringLength(50)]
        //public string? Status { get; set; }

        //[StringLength(50)]
        //public string? Prefix { get; set; }

        //[StringLength(50)]
        //public string? Suffiix { get; set; }

        //public float? Weight { get; set; }

        //[StringLength(50)]
        //public string? WeightUnit { get; set; }

        //public float? Dheight { get; set; }

        //public float? Dweight { get; set; }

        //public float? Dlength { get; set; }

        //public float? PartCodeType { get; set; }

        //[StringLength(800)]
        //public string? PurchaseName { get; set; }

        //[StringLength(50)]
        //public string? PurchaseCode { get; set; }

        //[StringLength(50)]
        //public string? Manufacture { get; set; }

        //public float? ProductionRate { get; set; }

        //public int? Maxmimumstock { get; set; }

        //public string? Description { get; set; }

        //[StringLength(50)]
        //public string? Commoditycode { get; set; }

        //public int? Category { get; set; }

        //public int? Subclassification { get; set; }

        //public int? Dimension { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime? RateDate { get; set; }

        //[StringLength(500)]
        //public string? BranchAC { get; set; }

        //[StringLength(500)]
        //public string? SalesledgerAC { get; set; }

        //[StringLength(500)]
        //public string? PurchaseledgerAC { get; set; }

        //[StringLength(100)]
        //public string? Vehiclemodel { get; set; }

        //[StringLength(500)]
        //public string? Labourcategory { get; set; }

        //[StringLength(50)]
        //public string? RackNo { get; set; }

        //[StringLength(50)]
        //public string? BinNo { get; set; }

        //public decimal? PartNo { get; set; }

        //public int? SalesledgerACId { get; set; }

        //public int? PurchaseLedgerACId { get; set; }

        //public int? BranchACId { get; set; }

        //public int? ItemsID { get; set; }

        //[StringLength(50)]
        //public string? ExciseableItem { get; set; }

        //[StringLength(800)]
        //public string? ItemBrand { get; set; }

        //[StringLength(100)]
        //public string? ItemColor { get; set; }

        //[StringLength(100)]
        //public string? ItemModel { get; set; }

        //[StringLength(50)]
        //public string? InventoryEffect { get; set; }

        //[StringLength(50)]
        //public string? YearID { get; set; }

        //public int? SchemeFlag { get; set; }

        //public string? ItemDescription { get; set; }

        //public float? ConvertQty { get; set; }

        //public int? ConvertUnit { get; set; }

        //public decimal? itemid { get; set; }

        //public int? ColorId { get; set; }

        //public int? Oldparentid { get; set; }

        //public int? OldSubclassification { get; set; }

        //[StringLength(200)]
        //public string? OldItemName { get; set; }

        //public int? OldItemId { get; set; }

        //public int? OItemId { get; set; }

        //public decimal? moncon { get; set; }

        //[StringLength(100)]
        //public string? HsnCode { get; set; }

        //[StringLength(800)]
        //public string? UnitName { get; set; }

        //public int? brandid { get; set; }

        //public string? ItemSpecification { get; set; }

        //public int? SizeId { get; set; }

        //[StringLength(200)]
        //public string? imagepath { get; set; }

        //public string? HINDIitem { get; set; }

        //public int? Classification { get; set; }

        //[StringLength(100)]
        //public string? CategoryName { get; set; }

        //public float? Cess { get; set; }

        //public float? RelationUnit1 { get; set; }

        //public float? RelationUnit2 { get; set; }

        //public float? RelationUnit3 { get; set; }

        //public float? Weight2 { get; set; }

        //public float? Weight3 { get; set; }

        //public int? Unit2 { get; set; }

        //public int? Unit3 { get; set; }

        //public int? Godown { get; set; }

        //public int? Godown2 { get; set; }

        //public int? Godown3 { get; set; }

        //public int? ExecComm { get; set; }

        //public int? GstID { get; set; }

        //public int? CessID { get; set; }

        //public string? parentitemname { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime? op_date { get; set; }

        //[StringLength(800)]
        //public string? parentitemcode { get; set; }

        //public float? GrossWeight { get; set; }

        //public float? NetWeight { get; set; }

        //public float? CartoonWeight { get; set; }

        //public float? PackagingQty { get; set; }

        //public float? MinOrderQty { get; set; }

        //public float? ExpGst { get; set; }

        //[StringLength(100)]
        //public string? ExpGstType { get; set; }

        //public int? Discountcategoryid { get; set; }

        //[StringLength(200)]
        //public string? Discountcategory { get; set; }

        //public string? countrylist { get; set; }

        //public string? Vendorlist { get; set; }

        //public string? ALIAS { get; set; }

        //public int? DesignNo { get; set; }

        //[StringLength(100)]
        //public string? subitemrequired { get; set; }

        //[StringLength(100)]
        //public string? Areacalculationrequired { get; set; }

        //[StringLength(200)]
        //public string? formularequired { get; set; }

        //public string? formula { get; set; }

        //public int? Serial { get; set; }

        //public int? purchaseunitid { get; set; }

        //public int? saleunitid { get; set; }

        //public float? discountpercent { get; set; }

        //public float? salerate { get; set; }

        //public float? alternateunitsalerate { get; set; }

        //public int? DesignId { get; set; }

        //public int? BaseColorId { get; set; }

        //[StringLength(200)]
        //public string? BaseColor { get; set; }

        //public float? BomRate { get; set; }

        //public float? MinRate1 { get; set; }

        //public float? MaxRate1 { get; set; }

        //public float? MinRate2 { get; set; }

        //public float? MaxRate2 { get; set; }

        //public float? MinRate3 { get; set; }

        //public float? MaxRate3 { get; set; }

        //public int? Equipmentid { get; set; }

        //[StringLength(200)]
        //public string? itemtype3 { get; set; }

        //public int? Frontpocketid { get; set; }

        //public int? Backpocketid { get; set; }

        //public float? Specialpricefactor { get; set; }

        //public float? Custompricefactor { get; set; }

        //public int? mrnrate { get; set; }

        //[StringLength(200)]
        //public string? @class { get; set; }

        //[StringLength(500)]
        //public string? plsn { get; set; }

        //[StringLength(500)]
        //public string? plsn1 { get; set; }

        //[StringLength(500)]
        //public string? asterisk { get; set; }

        //public float? Irate { get; set; }

        //public float? Nrate { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime? changedate { get; set; }

        //public int? departmentid { get; set; }

        //public float? volume { get; set; }

        //[StringLength(500)]
        //public string? ColorList { get; set; }

        //public float? MaxDiscount { get; set; }

        //public int? coatid { get; set; }

        //[StringLength(500)]
        //public string? Paintratio { get; set; }

        //public string? Skucode { get; set; }

        //public string? displayitemname { get; set; }

        //public int? QCCatId { get; set; }

        //[StringLength(100)]
        //public string? QCRequired { get; set; }

        //public int? PackingId { get; set; }

        //public float? Bhada { get; set; }

        //public int? ModelId { get; set; }

        //[StringLength(100)]
        //public string? SerialNo { get; set; }

        //public int? TypeId { get; set; }

        //public int? TypeId2 { get; set; }

        //public int? TypeId3 { get; set; }

        //public int? TypeId4 { get; set; }

        //public int? TypeId5 { get; set; }

        //[StringLength(500)]
        //public string? DisplayName { get; set; }

        //[StringLength(200)]
        //public string? TaxType { get; set; }

        //[StringLength(200)]
        //public string? ProductType { get; set; }

        //[StringLength(200)]
        //public string? VariationName { get; set; }

        //[StringLength(1000)]
        //public string? VariationValues { get; set; }

        //[StringLength(1000)]
        //public string? VariationSKU { get; set; }

        //[StringLength(1000)]
        //public string? VariationPurchasePriceIncludeTax { get; set; }

        //[StringLength(1000)]
        //public string? VariationPurchasePriceExcludeTax { get; set; }

        //[StringLength(1000)]
        //public string? VariationProfitMargin { get; set; }

        //[StringLength(1000)]
        //public string? VariarionSellingPrice { get; set; }

        //public float? vprofitmargin { get; set; }

        //public float? vpurchseexclude { get; set; }

        //[StringLength(1000)]
        //public string? varvalue { get; set; }

        //public string? branch { get; set; }

        //public int? MachineId { get; set; }

        //[StringLength(50)]
        //public string? pricelist { get; set; }

        //[StringLength(50)]
        //public string? srno { get; set; }

        //[StringLength(50)]
        //public string? Commission { get; set; }

        //public float? LedRate { get; set; }

        //public float? FillRate { get; set; }

        //public float? ExUPRate { get; set; }

        //[StringLength(50)]
        //public string? ExUPCommission { get; set; }

        //public float? ExUPLedRate { get; set; }

        //public float? ExUPFillRate { get; set; }

        //public float? NepalRate { get; set; }

        //[StringLength(50)]
        //public string? NepalCommission { get; set; }

        //public float? NepalLedRate { get; set; }

        //public float? NepalFillRate { get; set; }

        //public int? updateuserid { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime? updateuserdate { get; set; }

        //[StringLength(200)]
        //public string? DrawingNo { get; set; }

        //[StringLength(50)]
        //public string? AccessQtyReq { get; set; }

        //public float? AccessQty { get; set; }

        //public int? excelid { get; set; }

        //public int? tallystatus { get; set; }

        //public int? oldparent { get; set; }

        //public decimal? SNO { get; set; }

        //public int? AssetID { get; set; }

        //[StringLength(200)]
        //public string? AssetType { get; set; }

        //public int? backflag { get; set; }

        //public string? itemcodeback { get; set; }

        //[StringLength(200)]
        //public string? Specialpricefactor1 { get; set; }

        //public int? oldsubclassificationid { get; set; }

        //[StringLength(500)]
        //public string? PrintName { get; set; }

        //public string? ItemDescription1 { get; set; }

        //public string? ItemDescription2 { get; set; }

        //public string? ItemDescription3 { get; set; }

        //public string? ItemDescription4 { get; set; }

        //public int? busystatus { get; set; }

        //public string? Busyuniquecode { get; set; }

        //public float? breakagerate { get; set; }

        //public float? lossrate { get; set; }

        //public float? scarprate { get; set; }

        //public string? olditemcode { get; set; }

        //[StringLength(200)]
        //public string? Rate { get; set; }

        //public int? id { get; set; }

        //public int? productid { get; set; }

        //public int? vendorid { get; set; }

        //public string? vendorname { get; set; }

        //public int? maingroupid { get; set; }

        //public string? maingroupname { get; set; }

        //public int? subgroupid { get; set; }

        //public string? subgroupname { get; set; }

        //public string? servicename { get; set; }

        //public string? servicevalue { get; set; }

        //public string? specication { get; set; }

        //public string? specicationname { get; set; }

        //public int? ShowOnDastboard { get; set; }


        //public int? MainId { get; set; }
        public decimal? itemid { get; set; }
        public string? imagepath { get; set; }
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        [StringLength(1000)]
        public string? serviceimages { get; set; }
        [Column("Rate")]
        public string? Rate { get; set; }
        [Column("id")]
        public int? id { get; set; }
        [Column("productid")]
        public int? productid { get; set; }
        [Column("vendorid")]
        public int? vendorid { get; set; }
        [Column("vendorname")]
        public string? vendorname { get; set; }
        [Column("maingroupid")]
        public int? maingroupid { get; set; }
        [Column("maingroupname")]
        public string? maingroupname { get; set; }
        [Column("subgroupid")]
        public int? subgroupid { get; set; }
        [Column("subgroupname")]
        public string? subgroupname { get; set; }
        [Column("servicename")]
        public string? servicename { get; set; }
        [Column("servicevalue")]
        public string? servicevalue { get; set; }
        [Column("specication")]
        public string? specication { get; set; }
        [Column("specicationname")]
        public string? specicationname { get; set; }
        public int? serviceid { get; set; }  
        public int? ShowOnDastboard { get; set; }
        public int? websitestatus { get; set; }
		[NotMapped]
        public double Price { get; set; }
        [NotMapped]
        public List<Rate> RateList { get; set; }
        [NotMapped]
        public double? ItemRate { get; set; }
		[NotMapped]
		public double? Tax { get; set; }
		[NotMapped]
		public double? DiscountPercent { get; set; }
		[NotMapped]
		public double? DiscountAmount { get; set; }
		[NotMapped]
		public string? ServiceSeoUrl { get; set; }
        [NotMapped]
        public string? ItemRateForDisplay { get; set; }
    }
    #region ItemDetails
    public class ItemDeatsils
    {
        public decimal? itemid { get; set; }
        public string? imagepath { get; set; }
        public string? ItemName { get; set; }
        public string? ItemCode { get; set; }
        [StringLength(1000)]
        public string? serviceimages { get; set; }
        [Column("Rate")]
        public string? Rate { get; set; }
        [Column("id")]
        public int? id { get; set; }
        [Column("productid")]
        public int? productid { get; set; }
        [Column("vendorid")]
        public int? vendorid { get; set; }
        [Column("vendorname")]
        public string? vendorname { get; set; }
        [Column("maingroupid")]
        public int? maingroupid { get; set; }
        [Column("maingroupname")]
        public string? maingroupname { get; set; }
        [Column("subgroupid")]
        public int? subgroupid { get; set; }
        [Column("subgroupname")]
        public string? subgroupname { get; set; }
        [Column("servicename")]
        public string? servicename { get; set; }
        [Column("servicevalue")]
        public string? servicevalue { get; set; }
        [Column("specication")]
        public string? specication { get; set; }
        [Column("specicationname")]
        public string? specicationname { get; set; }
        public int? ShowOnDastboard { get; set; }
        public string? ServiceSeoUrl { get; set; }  
		[NotMapped]
        public double Price { get; set; }
        public int? Quantity { get; set; }   
        public double? Tax { get; set; }
        public double? ItemRate { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountAmount { get; set; }
        public string? GroupType { get; set; }
        [NotMapped]
        public string? ItemRateForDisplay { get; set; }
        public List<packagemasterdto> packagemasters { get; set; } 
        public List<Packagetransdto> packageTrans { get; set; }
    }
    #endregion

    #region Rate
    public class Rate
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    #endregion

    #region SerachItemdetails
    public class SerachItemdetails
    {
        public Item Item { get; set; }
        public List<RatingReview> Reviews { get; set; }
        public VendorDetails Vendor { get; set; }

    }
    #endregion

    public class PlaceEnquiryRequest
    {
        public List<int> ItemIds { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }    
        public string State { get; set; }   
        public string Country { get; set; } 
        public string ZipCode { get; set; } 
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

}
