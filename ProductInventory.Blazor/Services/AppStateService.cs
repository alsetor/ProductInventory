using ProductInventory.Blazor.Models;

namespace ProductInventory.Blazor.Services
{
    public class AppStateService
    {
        private readonly Dictionary<string, List<Product>> _productCache = new();

        public ProductFilterState FilterState { get; set; } = new();

        public bool TryGetCachedProducts(out List<Product> products)
        {
            string key = GetCacheKey(FilterState);
            return _productCache.TryGetValue(key, out products);
        }

        public void CacheProducts(List<Product> products)
        {
            string key = GetCacheKey(FilterState);
            _productCache[key] = products;
        }

        public void Reset()
        {
            _productCache.Clear();
            FilterState.Clear();
        }

        private string GetCacheKey(ProductFilterState filter)
        {
            return $"{filter.Page}_{filter.Name}_{filter.MinPrice}_{filter.MaxPrice}";
        }
    }

}
