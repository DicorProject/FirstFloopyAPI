using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class BlogMaster
    {
        [Column("id")]
        public int? Id { get; set; }

        [Column("MainId")]
        public int? MainId { get; set; }

        [Column("SubId")]
        public int? SubId { get; set; }

        [Column("BlogDescription")]
        [MaxLength]
        public string? BlogDescription { get; set; }

        [Column("image")]
        [MaxLength]
        public string? Image { get; set; }

        [Column("Status")]
        public int? Status { get; set; }

        [Column("CompId")]
        public int? CompId { get; set; }

        [Column("BranchId")]
        public int? BranchId { get; set; }

        [Column("Userid")]
        public int? Userid { get; set; }

        [Column("YearId")]
        [StringLength(10)]
        public string? YearId { get; set; }

        [Column("Tittle")]
        [StringLength(300)]
        public string? Tittle { get; set; }

        [Column("Author")]
        [StringLength(300)]
        public string? Author { get; set; }
        [NotMapped]
        public string? CategoryName { get; set; }   
    }

    #region BlogDetailsResponseModel
    public class BlogDetails
    {
        public int? BlogId { get; set; }  
        public string? Title { get; set; }
        public int? SubId { get; set; }
        public int? MainId { get; set; }
        public string? BlogDescription { get; set; }
        public string? image { get; set; }
        public int? Status { get; set; }
        public string? CategoryName { get; set; }        
        public List<BlogTrans> blogTrans { get; set; }  
    }
    #endregion

    #region BlogCountwithListResponseModel
    public class BlogCountwithList
    {
        public List<BlogMaster> blogs { get; set; }
        public int TotalBlogs { get; set; }
    }
    #endregion
}
