namespace Shop.UI.Models
{
    public class ProductGetAllResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public decimal SalePrice { get; set; }
        public string? ImageUrl { get; set; }
    }
}
