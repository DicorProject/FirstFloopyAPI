using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class LeadServices
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("LeadId")]
        public int LeadId { get; set; }

        [Column("ServiceId")]
        public int ServiceId { get; set; }

        [Column("Rate")]
        public decimal Rate { get; set; }

        [Column("EntryDate")]
        public DateTime EntryDate { get; set; }

        [Column("CompId")]
        public int CompId { get; set; }

        [Column("BranchId")]
        public int BranchId { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("YearId")]
        public int YearId { get; set; }
    }
}
