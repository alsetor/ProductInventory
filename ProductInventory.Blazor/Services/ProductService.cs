using System.Net.Http.Json;
using ProductInventory.Blazor.Models;

namespace ProductInventory.Blazor.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly AppStateService _appState;

        public ProductService(HttpClient httpClient, AppStateService state)
        {
            _httpClient = httpClient;
            _appState = state;
        }

        public async Task LoadProductsAsync(bool resetState = false)
        {
            if (resetState)
                _appState.Reset();

            if (_appState.TryGetCachedProducts(out var cached))
                return;

            var f = _appState.FilterState;
            var query = $"?name={f.Name}&minPrice={f.MinPrice}&maxPrice={f.MaxPrice}&page={f.Page}";
            var response =  await _httpClient.GetFromJsonAsync<ProductListResponse>($"api/products{query}") ?? new ProductListResponse();
            if (response != null)
            {
                _appState.CacheProducts(response.Products);
                _appState.FilterState.TotalCount = response.TotalCount;
            }
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", product);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}


