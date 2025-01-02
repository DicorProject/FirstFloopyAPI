namespace Floppy.Domain.Entities
{
    public class Url
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string UrlAddress { get; set; }
    }

    #region CategoryData
    public class CategoryData
    {
        public string CategoryName { get; set; }
        public List<SubCategoryData> SubCategories { get; set; }
    }
    #endregion

    #region SubCategoryData
    public class SubCategoryData
    {
        public string SubCategoryName { get; set; }
        public string FirstPrice { get; set; }
    }
    #endregion
}
