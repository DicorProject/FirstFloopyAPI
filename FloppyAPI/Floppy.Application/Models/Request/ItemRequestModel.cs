namespace Floppy.Application.Models.Request
{
    public class ItemRequestModel
    {
        public int CategoryId { get; set; }
        public List<int> SubCategoryIds { get; set; }
        public List<string> ServicesName { get; set; }      
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int PageSize { get; set; }
        public int StartIndex { get; set; }
    }
}
