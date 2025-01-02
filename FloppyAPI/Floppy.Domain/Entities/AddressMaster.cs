namespace Floppy.Domain.Entities
{
    public class AddressMaster
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? AddressType { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? Area { get; set; }
        public string? Country { get; set; }
        public string? StateCode { get; set; }  
        public string? CountryCode { get; set;}
    }

    public class AddressMasterData
    {
        public int UserId { get; set; }
        public string? AddressType { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? StateCode { get; set; }
        public string? PinCode { get; set; }
        public string? Area { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
    }

}
