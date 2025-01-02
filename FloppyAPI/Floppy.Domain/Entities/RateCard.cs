using System.ComponentModel.DataAnnotations;

namespace Floppy.Domain.Entities
{
    public class RateCard
    {
        public int id { get; set; }

        public int? maincatid { get; set; }

        public int? subcatid { get; set; }

        [StringLength(300)]
        public string Rate { get; set; }

        public int? Userid { get; set; }

        public int? CompId { get; set; }

        [StringLength(20)]
        public string YearId { get; set; }

        public int? Branchid { get; set; }
        public string? Sparepartname { get; set; }  
    }
}
