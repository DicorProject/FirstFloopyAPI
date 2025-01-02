using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class Category
    {
        [Column("ClaasificationName")]
        [StringLength(200)]
        public string? ClaasificationName { get; set; }

        [Column("EnteryDate")]
        public DateTime? EnteryDate { get; set; }

        [Column("MainId")]
        public decimal MainId { get; set; }

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

        [Column("OldClassificationName")]
        [StringLength(200)]
        public string? OldClassificationName { get; set; }

        [Column("OldMainId")]
        public int? OldMainId { get; set; }

        [Column("image")]
        [StringLength(300)]
        public string? Image { get; set; }

        [Column("Code")]
        [StringLength(500)]
        public string? Code { get; set; }

        [Column("imageupload")]
        public string? ImageUpload { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("url")]
        public string? Url { get; set; }
        [Column("Status")]
        public int Status { get; set; }
        [Column("ShowOnDastboard")]
        public int? ShowOnDastboard { get; set; }
        [Column("Categoryseourl")]
        public string? Categoryseourl{ get; set; } 
        public string? GroupType { get; set; }
        public string? googelname { get; set; }

	}

    #region CategoryDtoResponseModel
    public class CategoryDto
    {
        public decimal? MainId { get; set; }
        public string? ClassificationName { get; set; }
        public  string? Categoryseourl { get;set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public int? Status { get; set; }
        public int? ShowOnDashboard { get; set; }
        public List<SubcategoryDto> Subcategories { get; set; }
    }
    #endregion

    #region SubcategoryDtoResponseModel
    public class SubcategoryDto
    {
        public decimal? MainId { get; set; }
        public decimal? SubId { get; set; }
        public string? SubClassificationName { get; set; }
        public string? NewUrl { get; set; }
        public string? Image { get; set; }
        public string? Url { get; set; }
        public string? GoogleName { get; set; }
        public int? Status { get; set; }
        public int? ShowOnDashboard { get; set; }
    }
    #endregion
}
