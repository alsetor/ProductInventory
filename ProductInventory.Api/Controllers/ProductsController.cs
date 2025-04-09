using Microsoft.AspNetCore.Mvc;
using ProductInventory.Api.Models;
using ProductInventory.Api.Repositories;

namespace ProductInventory.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
        {
            var productListInfo = await _repository.GetProductsAsync(filter);
            return Ok(productListInfo);
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            var createdProduct = await _repository.AddAsync(product);
            return CreatedAtAction(nameof(Add), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();
            await _repository.UpdateAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
