using Floppy.Domain.Entities;

namespace Floppy.Application.Models.Response
{
    public class ServiceDetailsResponse
    {
        public ItemDeatsils item {  get; set; } 
        public List<RatingReview> ratingReview { get; set; }  
    }
}
