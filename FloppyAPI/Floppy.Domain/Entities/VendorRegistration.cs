using System.ComponentModel.DataAnnotations.Schema;

namespace Floppy.Domain.Entities
{
    public class VendorRegistration
    {
        public int? id { get; set; }
        public string? ServiceProvider { get; set; }
        public string? Address { get; set; }
        public int? Userid { get; set; }
        public string? ContactPerson { get; set; }
        public string? Area { get; set; }
        public string? Aboutus { get; set; }
        public string? term_condition { get; set; }
        public string? logo_img { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
        public DateTime? Entrydate { get; set; }
        public DateTime? datetimespent { get; set; }
        public string? CityName { get; set; }
        public int? Cityid { get; set; }
        public string? verifystatus { get; set; }
        public string? registrationfor { get; set; }
        public string? registervendor { get; set; }
        public string? USER_ID { get; set; }
        public int? VendorCode { get; set; }
        public string? compextensionnumber { get; set; }
        public string? longitude { get; set; }
        public string? latitude { get; set; }
        public string? mapurl { get; set; }
        public int? srno { get; set; }
        public int? oldpartyid { get; set; }
        public string? flag { get; set; }
        public int? wallet { get; set; }
        public int? CompId { get; set; }
        public string? YearId { get; set; }
        public int? Branchid { get; set; }
        public int? Country { get; set; }
        public int? State { get; set; }
        public int? ZipCode { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessAddress { get; set; }
        public string? CityTown { get; set; }
        public string? BussinessRegistrationNumber { get; set; }
        public int? CompanyType { get; set; }
        public string? Status { get; set; }
        [NotMapped]
        public string? SubGroupName { get; set; }
        [NotMapped]
        public double? AverageRating { get; set; }
        [NotMapped]
        public int TotalServices { get; set; }
        [NotMapped]
        public List<string> servicesName { get; set; }  
    }
    #region VendorDetails
    public class VendorDetails
    {
        public int? Id { get; set; }
        public string VendorName { get; set; }
        public double Distance { get; set; }
        public string Address { get; set; }
        public string Status { get; set; } 
    }
    #endregion
}
