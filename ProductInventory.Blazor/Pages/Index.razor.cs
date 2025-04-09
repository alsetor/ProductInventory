using Microsoft.Extensions.Primitives;
using MudBlazor;
using ProductInventory.Blazor.Components;
using ProductInventory.Blazor.Models;
using ProductInventory.Blazor.Services;

namespace ProductInventory.Blazor.Pages
{
    public partial class Index
    {
        public readonly IProductService _productService;

        public Index(IProductService productService)
        {
            _productService = productService;
        }

        protected override async Task OnInitializedAsync()
        {
            ParseQueryParams();

            if (!AppState.TryGetCachedProducts(out _))
            {
                await _productService.LoadProductsAsync();
            }
        }

        private async Task OnPageChanged(int newPage)
        {
            AppState.FilterState.Page = newPage;
            await _productService.LoadProductsAsync();
            UpdateUrlFromFilters();
        }

        private async Task ApplyFilters()
        {
            AppState.FilterState.Page = 1;
            await _productService.LoadProductsAsync();
            UpdateUrlFromFilters();
        }

        private async Task OpenProductDialog(Product product)
        {
            var isNew = product.Id == 0;
            var parameters = new DialogParameters<ProductDialog> { { x => x.product, product } };

            var dialog = await DialogService.ShowAsync<ProductDialog>(isNew ? "Add Product" : "Edit Product", parameters);
            var result = await dialog.Result;

            if (result.Canceled) return;

            var success = isNew
                ? await _productService.AddProductAsync(product)
                : await _productService.UpdateProductAsync(product);

            Snackbar.Add(success ? $"{(isNew ? "Added" : "Updated")} product successfully!" : $"Failed to {(isNew ? "add" : "update")} product.",
                         success ? Severity.Success : Severity.Error);

            if (success)
                await _productService.LoadProductsAsync(resetState: true);
        }

        private async Task OpenProductDetailsDialog(Product product)
        {
            var parameters = new DialogParameters<ProductDetails> { { x => x.product, product } };
            await DialogService.ShowAsync<ProductDetails>("Product Details", parameters);
        }

        private async Task DeleteProduct(int productId)
        {
            var parameters = new DialogParameters<CancelOkDialog> { { x => x.message, "Are you sure want to delete?" } };
            var dialog = await DialogService.ShowAsync<CancelOkDialog>("Delete Product", parameters);
            var result = await dialog.Result;

            if (result.Canceled) return;

            var success = await _productService.DeleteProductAsync(productId);
            Snackbar.Add(success ? "Product deleted successfully!" : "Failed to delete product.",
                         success ? Severity.Success : Severity.Error);

            if (success)
                await _productService.LoadProductsAsync(resetState: true);
        }

        private void ParseQueryParams()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            AppState.FilterState.Name = queryParams.TryGetValue("name", out var name) ? name : (string?)null;
            AppState.FilterState.MinPrice = TryParseDecimal(queryParams, "minPrice");
            AppState.FilterState.MaxPrice = TryParseDecimal(queryParams, "maxPrice");
            AppState.FilterState.Page = queryParams.TryGetValue("page", out var page) && int.TryParse(page, out var pg) ? pg : 1;
        }

        private void UpdateUrlFromFilters()
        {
            var uriBuilder = new UriBuilder(Navigation.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

            query["name"] = AppState.FilterState.Name ?? string.Empty;
            query["minPrice"] = AppState.FilterState.MinPrice?.ToString() ?? string.Empty;
            query["maxPrice"] = AppState.FilterState.MaxPrice?.ToString() ?? string.Empty;
            query["page"] = AppState.FilterState.Page.ToString();

            uriBuilder.Query = query.ToString();
            Navigation.NavigateTo(uriBuilder.ToString(), forceLoad: false);
        }

        private decimal? TryParseDecimal(Dictionary<string, StringValues> query, string key)
            => query.TryGetValue(key, out var value) && decimal.TryParse(value, out var result) ? result : null;
    }
}