using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class Parameter
    {
        [Column("ParameterName")]
        public string? ParameterName { get; set; }

        [Column("ParameterDetail")]
        public string? ParameterDetail { get; set; }

        [Column("ParentId")]
        public int? ParentId { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("Paraid")]
        public int? Paraid { get; set; }

        [Column("DateTimesTamp")]
        public DateTime? DateTimesTamp { get; set; }

        [Column("DateTimesTampNo")]
        public decimal? DateTimesTampNo { get; set; }

        [Column("CompId")]
        public int? CompId { get; set; }

        [Column("BranchId")]
        public int? BranchId { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("SubDetail")]
        public string? SubDetail { get; set; }

        [Column("ImageName")]
        public string? ImageName { get; set; }

        [Column("SeqNo")]
        public int? SeqNo { get; set; }

        [Column("PId")]
        public int? PId { get; set; }

        [Column("maincategory")]
        public string? MainCategory { get; set; }

        [Column("subcategory")]
        public string? Subcategory { get; set; }
    }

}
