using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class PackageTrans
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? TransId { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string ProductName { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? ProductValue { get; set; }

        public int? UserId { get; set; }

        public int? CompId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? YearId { get; set; }

        public int? BranchId { get; set; }
    }


    #region Packagetransdto
    public class Packagetransdto
    {
        public string ProductName { get; set; }
        public string? ProductValue { get; set; }
    }

    #endregion
}
