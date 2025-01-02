using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class LeadVandorTrans
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("Vendorid")]
        public int? VendorId { get; set; }

        [Column("Leadid")]
        public int? LeadId { get; set; }

        [Column("Compid")]
        public int? CompId { get; set; }

        [Column("Branchid")]
        public int? BranchId { get; set; }

        [Column("Userid")]
        public int? UserId { get; set; }

        [Column("Yearid")]
        public string? YearId { get; set; }
    }
}
