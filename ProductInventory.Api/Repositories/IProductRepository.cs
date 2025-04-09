using ProductInventory.Api.Models;

namespace ProductInventory.Api.Repositories
{
    public interface IProductRepository
    {
        Task<ProductListResponse> GetProductsAsync(ProductFilter filter);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}