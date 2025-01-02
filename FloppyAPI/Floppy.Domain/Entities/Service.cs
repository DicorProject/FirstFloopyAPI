using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }

        [Column("maincatid")]
        public int? MainCatId { get; set; }

        [Column("maincategory")]
        [StringLength(4000)]
        public string? MainCategory { get; set; }

        [Column("subcatid")]
        public int? SubCatId { get; set; }

        [Column("subcategory")]
        [StringLength(4000)]
        public string? SubCategory { get; set; }

        [Column("servicename")]
        [StringLength(4000)]
        public string? ServiceName { get; set; }

        [Column("specication")]
        public int? Specification { get; set; }

        [Column("specicationname")]
        [StringLength(4000)]
        public string? SpecificationName { get; set; }

        [Column("Userid")]
        public int? UserId { get; set; }

        [Column("CompId")]
        public int? CompId { get; set; }

        [Column("YearId")]
        [StringLength(20)]
        public string? YearId { get; set; }

        [Column("Branchid")]
        public int? BranchId { get; set; }
		public double? Rate { get; set; }
		public double? GST { get; set; }
		[Column("ServiceSeoUrl")]
		public string? ServiceSeoUrl { get; set; }
	}
}
