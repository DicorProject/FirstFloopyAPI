using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class HomePageSlider
    {
        public int? Id { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Image { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? URL { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Tittle { get; set; }

        public int? Status { get; set; }

        public int? Seqno { get; set; }

        public int? Compid { get; set; }

        public int? UserId { get; set; }

        public int? BranchId { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? Yearid { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? Type { get; set; }
    }

    #region  SubgroupModel
    public class SubgroupModel
    {
        public string SubgroupName { get; set; }
        public int? SubgroupId { get; set; }        
    }
    #endregion

    #region AreaModel
    public class AreaModel
    {
        public string AreaName { get; set; }   
        public List<SubgroupModel> Subgroups { get; set; } = new List<SubgroupModel>();
    }
    #endregion

    #region CityModel
    public class CityModel
    {
        public string CityName { get; set; }
        public List<AreaModel> Areas { get; set; } = new List<AreaModel>();
    }
    #endregion
}
