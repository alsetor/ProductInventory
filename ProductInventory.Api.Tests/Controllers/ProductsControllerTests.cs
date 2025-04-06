using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductInventory.Api.Controllers;
using ProductInventory.Api.Models;
using ProductInventory.Api.Repositories;

namespace ProductInventory.Api.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _controller = new ProductsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult()
        {
            var productListInfo = new ProductListResponse
            {
                Products = new List<Product>
                {
                    new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10, Quantity = 100 },
                    new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20, Quantity = 200 }
                },
                TotalCount = 2
            };

            var filter = new ProductFilter();

            _mockRepo.Setup(repo => repo.GetProductsAsync(filter)).ReturnsAsync(productListInfo);

            var result = await _controller.GetProducts(filter);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count);
        }

        [Fact]
        public async Task GetProductById_ExistingId_ReturnsOkResult()
        {
            var product = new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10, Quantity = 100 };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.GetById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(product.Id, returnProduct.Id);
        }

        [Fact]
        public async Task AddProduct_ValidProduct_ReturnsCreatedResult()
        {
            var product = new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10, Quantity = 100 };
            
            var result = await _controller.Add(product);
            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdResult.ActionName);
        }

        [Fact]
        public async Task UpdateProduct_ExistingProduct_ReturnsNoContent()
        {
            var product = new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10, Quantity = 100 };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.Update(1, product);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ExistingId_ReturnsNoContent()
        {
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);
            var product = new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10, Quantity = 100 };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }


    }
}
