using Floppy.Domain.Entities;

namespace Floppy.Application.Models.Response
{
    public class ItemWithVendorDetails
    {
        public Item Item { get; set; }
        public VendorDetails Vendor { get; set; }
        public List<RatingReview> Reviews { get; set; }
    }
    public class ItemWithVendorDetailsResponse
    {
        public List<ItemWithVendorDetails> Items { get; set; }  
        public int TotalItems { get; set; }     
    }
}
