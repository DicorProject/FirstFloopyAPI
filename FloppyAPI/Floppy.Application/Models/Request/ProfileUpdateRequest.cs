namespace Floppy.Application.Models.Request
{
    public class ProfileUpdateRequest
    {
        public int UserId { get; set; } 
        public string Name { get; set; }    
        public string Phone {  get; set; }
        public string Pincode { get; set; } 
        public string Locality { get; set; }    
        public string Address { get; set; } 
        public string State { get; set; }   
        public string City { get; set; }
        public string? Image { get; set; } 
    }
}
