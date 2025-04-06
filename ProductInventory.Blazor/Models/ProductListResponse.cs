namespace ProductInventory.Blazor.Models;

public class ProductListResponse
{
    public List<Product> Products { get; set; } = new();
    public int TotalCount { get; set; } = 0;

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / 10);
}
