namespace Floppy.Application.Models.Request
{
    public class SearchItemRequestModel
    {
        public List<int?> subgroupIds { get; set; }
        public List<string> serviceNames { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

	}

}
