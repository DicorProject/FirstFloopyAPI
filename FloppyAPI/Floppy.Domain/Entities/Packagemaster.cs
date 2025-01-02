namespace Floppy.Domain.Entities
{
    public class Packagemaster
    {
        public int? id { get; set; }
        public int? productid { get; set; }
        public int? vendorid { get; set; }
        public string? vendorname { get; set; }
        public int? maingroupid { get; set; }
        public string? maingroupname { get; set; }
        public int? subgroupid { get; set; }
        public string? subgroupname { get; set; }
        public string? servicename { get; set; }
        public string? servicevalue { get; set; }
        public int? specication { get; set; }
        public string? specicationname { get; set; }
        public int? Userid { get; set; }
        public int? CompId { get; set; }
        public string? YearId { get; set; }
        public int? Branchid { get; set; }
    }


    #region packagemasterdto
    public class packagemasterdto
    {
        public int? productid { get; set; }
        public int? vendorid { get; set; }
        public string? vendorname { get; set; }
        public string? servicename { get; set; }
        public string? servicevalue { get; set; }
        public int? specication { get; set; }
        public string? specicationname { get; set; }
    }
    #endregion
}
