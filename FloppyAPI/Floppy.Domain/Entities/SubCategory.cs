using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class SubCategory
    {
        [Column("MainId")]
        public int? MainId { get; set; }

        [Column("SubClassificationName")]
        [StringLength(200)]
        public string? SubClassificationName { get; set; }

        [Column("SubId")]
        public decimal? SubId { get; set; }

        [Column("ClassId")]
        public decimal? ClassId { get; set; }

        [Column("CompanyId")]
        public int? CompanyId { get; set; }

        [Column("BranchId")]
        public int? BranchId { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("DateTimesTamp")]
        public DateTime? DateTimesTamp { get; set; }

        [Column("DateTimesTampNo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal DateTimesTampNo { get; set; }

        [Column("ClassificationCode")]
        [StringLength(50)]
        public string? ClassificationCode { get; set; }

        [Column("OldSubClassificationName")]
        [StringLength(200)]
        public string? OldSubClassificationName { get; set; }

        [Column("OldMainId")]
        public int? OldMainId { get; set; }

        [Column("OldSubId")]
        public int? OldSubId { get; set; }

        [Column("image")]
        [StringLength(500)]
        public string? Image { get; set; }

        [Column("Code")]
        [StringLength(500)]
        public string? Code { get; set; }

        [Column("MachineId")]
        public int? MachineId { get; set; }

        [Column("tabscroller")]
        [StringLength(200)]
        public string? Tabscroller { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        [Column("googelname")]
        [StringLength(200)]
        public string? Googelname { get; set; }

        [Column("newurl")]
        public string? Newurl { get; set; }

        [Column("newrurl")]
        [StringLength(1000)]
        public string? Newrurl { get; set; }
        [Column("Status")]
        public int? Status { get; set; }
        [Column("ShowOnDastboard")]
        public int? ShowOnDastboard { get; set; }   
    }
}
