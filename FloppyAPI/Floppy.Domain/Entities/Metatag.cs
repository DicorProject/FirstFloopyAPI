using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class Metatag
    {
        [Column("Page")]
        public string? Page { get; set; }

        [Column("Title")]
        public string? Title { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("Keywords")]
        public string? Keywords { get; set; }

        [Column("menuid")]
        public int? MenuId { get; set; }

        [Column("tempdatetime")]
        public DateTime? TempDateTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("datetimetempno")]
        public int DateTimeTempNo { get; set; }

        [Column("pdescription")]
        public string? PDescription { get; set; }

        [Column("Userid")]
        public int? UserId { get; set; }

        [Column("CompId")]
        public int? CompId { get; set; }

        [Column("YearId")]
        public string? YearId { get; set; }

        [Column("Branchid")]
        public int? BranchId { get; set; }
        public string? image { get; set; }
        public string? author { get; set; }
        [NotMapped]
        public List<string> Images { get; set; }
    }
}
