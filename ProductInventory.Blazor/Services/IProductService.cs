using ProductInventory.Blazor.Models;

namespace ProductInventory.Blazor.Services
{
    public interface IProductService
    {
        Task LoadProductsAsync(bool resetState = false);
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}
