using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class Testimonial
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string? CustumerName { get; set; }

        [StringLength(50)]
        public string CustomerDesigination { get; set; }

        public string Image { get; set; }

        public int? Rate { get; set; }

        public string? Review { get; set; }

        public int? Status { get; set; }

        public int? Compid { get; set; }

        public int? UserId { get; set; }

        public int? BranchId { get; set; }

        [StringLength(10)]
        public string? YearId { get; set; }
    }
}
