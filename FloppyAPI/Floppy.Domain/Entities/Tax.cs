using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class Tax
    {
        [Column("Fees")]
        public double? Fees { get; set; }
    }
}
