namespace Floppy.Domain.Entities
{
    public class Footer
    {
        public int? Id { get; set; }
        public int? Type { get; set; }
        public string Tittle { get; set; } 
        public int? SeqNo { get; set; }
        public string URL { get; set; }
        public int? Status { get; set; }
        public int? Compid { get; set; }
        public int? Userid { get; set; }
        public int? BranchID { get; set; }
        public string YearId { get; set; }
    }

    #region FooterWithParametersDtoResponseModel
    public class FooterWithParametersDto
    {
        public int? Id { get; set; }
        public int? SeqNo { get; set; }
        public string Tittle { get; set; }
        public string URL { get; set; }
        public string YearId { get; set; }
        public int? Status { get; set; }
    }
    #endregion

    #region FooterTypeGroupDtoModel
    public class FooterTypeGroupDto
    {
        public int Type { get; set; }
        public string? ParameterName { get; set; }
        public string? ParameterDetail { get; set; }
        public List<FooterWithParametersDto> Footers { get; set; }
    }

    #endregion
}
