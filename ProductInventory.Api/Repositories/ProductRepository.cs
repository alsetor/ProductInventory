using ProductInventory.Api.Data;
using ProductInventory.Api.Models;
using ProductInventory.Api.Data.Helpers;

namespace ProductInventory.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<ProductListResponse> GetProductsAsync(ProductFilter filter)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name));
            }

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            var totalCount = query.Count();
            var products = query
                .Skip((filter.Page - 1) * 10)
                .Take(10)
                .ToList();

            return new ProductListResponse()
            {
                Products = products,
                TotalCount = totalCount,
            };
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            _context.DetachLocal(existingProduct, product.Id);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
