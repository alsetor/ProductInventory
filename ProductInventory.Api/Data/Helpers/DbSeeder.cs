using System.Text.Json;
using ProductInventory.Api.Models;

namespace ProductInventory.Api.Data.Helpers
{
    public static class DbSeeder
    {
        public static void SeedFromJson(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

            if (context.Products.Any()) return;

            var json = File.ReadAllText("products_data.json");
            var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (products != null)
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
