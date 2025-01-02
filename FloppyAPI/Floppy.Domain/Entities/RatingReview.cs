using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class RatingReview
    {
        [Column("id")]
        public int? Id { get; set; }

        [Column("partyid")]
        public int? PartyId { get; set; }

        [Column("entrydate")]
        public DateTime? EntryDate { get; set; }

        [Column("itemid")]
        public int? ItemId { get; set; }

        [Column("entrytype")]
        public int? EntryType { get; set; }

        [Column("rating")]
        public string? Rating { get; set; }

        [Column("review")]
        public string? Review { get; set; }

        [Column("type")]
        [StringLength(50)]
        public string? Type { get; set; }

        [Column("approvestatus")]
        public int? ApproveStatus { get; set; }

        [Column("Name")]
        [StringLength(800)]
        public string? Name { get; set; }

        [Column("ratingvalue")]
        public int? RatingValue { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }
        [Column("userid")]
        public int? UserId { get; set; }    
    }

    #region RatingReviewDto
    public class RatingReviewDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public string? Rating { get; set; }
        public string? Review { get; set; }
        public int? RatingValue { get; set; }
        public int? ItemId { get; set; }
        public string Type { get; set; }
        public int? UserId { get; set; }
    }

    #endregion

    #region RatingReviewWithUserInfo
    public class RatingReviewWithUserInfo
    {
        public int? Id { get; set; }
        public int? PartyId { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? ItemId { get; set; }
        public int? EntryType { get; set; }
        public string? Rating { get; set; }
        public string? Review { get; set; }
        public string? Type { get; set; }
        public int? ApproveStatus { get; set; }
        public string? Name { get; set; }
        public int? RatingValue { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? UserId { get; set; }
        public string? Address { get; set; }
        public string? Locality { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? Image { get; set; }
    }

    #endregion
}
