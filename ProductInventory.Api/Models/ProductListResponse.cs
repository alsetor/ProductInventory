namespace ProductInventory.Api.Models
{
    public class ProductListResponse
    {
        public List<Product> Products { get; set; } = new();
        public int TotalCount { get; set; } = 0;
    }
}