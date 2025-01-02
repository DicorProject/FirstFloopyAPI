using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class TemplateMaster
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Subject")]
        public string? Subject { get; set; }

        [Column("Message")]
        public string? Message { get; set; }

        [Column("smtpid")]
        public int? Smtpid { get; set; }

        [Column("Smtp")]
        public string? Smtp { get; set; }

        [Column("EntryDate")]
        public DateTime? EntryDate { get; set; }

        [Column("CompId")]
        public int? CompId { get; set; }

        [Column("BranchId")]
        public int? BranchId { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("YearId")]
        public string? YearId { get; set; }

        [Column("Templatetype")]
        public string? Templatetype { get; set; }

        [Column("Documenttype")]
        public string? Documenttype { get; set; }

        [Column("Flag")]
        public string? Flag { get; set; }
    }
}
