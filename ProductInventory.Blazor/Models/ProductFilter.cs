namespace ProductInventory.Blazor.Models
{
    public class ProductFilter
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; } = 1;
    }
}
