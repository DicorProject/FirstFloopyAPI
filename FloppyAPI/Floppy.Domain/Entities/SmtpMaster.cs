using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class SmtpMaster
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Login")]
        public string? Login { get; set; }

        [Column("Password")]
        public string? Password { get; set; }

        [Column("Host")]
        public string? Host { get; set; }

        [Column("PortNo")]
        public string? PortNo { get; set; }

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

        [Column("Name")]
        public string? Name { get; set; }

        [Column("apikey")]
        public string? Apikey { get; set; }

        [Column("DepartID")]
        public int? DepartID { get; set; }

        [Column("Type")]
        public string? Type { get; set; }

        [Column("oldpassword")]
        public string? OldPassword { get; set; }
    }
}
