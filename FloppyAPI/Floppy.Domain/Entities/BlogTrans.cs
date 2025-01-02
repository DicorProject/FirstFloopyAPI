using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class BlogTrans
    {
        [Column("Id")]
        public int? Id { get; set; }

        [Column("Blogid")]
        public int? Blogid { get; set; }

        [Column("UserReview")]
        public int? UserReview { get; set; }

        [Column("ShowOnDashBord")]
        public int? ShowOnDashBord { get; set; }

        [Column("EntryDate")]
        public DateTime? EntryDate { get; set; }

        [Column("Compid")]
        public int? Compid { get; set; }

        [Column("Userid")]
        public int? Userid { get; set; }

        [Column("BranchId")]
        public int? BranchId { get; set; }

        [Column("YearId")]
        [StringLength(10)]
        public string? YearId { get; set; }
        [Column("Name")]
        public string? Name { get; set; }
        [Column("Email")]
        public string? Email { get; set; }
        [Column("Website")]
        public string? Website { get; set; }
        [Column("IsSaveNameEmailandWebsite")]
        public int? IsSaveNameEmailandWebsite { get; set; }
        [Column("Comment")]
        public string Comment { get; set; }
    }

    #region  BlogReviewDtoModel
    public class BlogReviewDto
    {
        public int? BlogId { get; set; }
        public int? UserReview { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public int IsSaveNameEmailandWebsite { get; set; }
        public string Comment { get; set; }
    }
    #endregion
}
