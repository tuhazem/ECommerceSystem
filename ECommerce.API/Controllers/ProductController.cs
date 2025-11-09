using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;

        public ProductController(IProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {

            var product = await service.GetAllAsync();
            return Ok(product);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) {

            var product = await service.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO dto) {

            var product = await service.AddAsync(dto);
            return Ok(product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDTO dto) {

            var product = await service.UpdateAsync(id, dto);
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id) { 
        
            await service.DeleteAsync(id);
            return NoContent();
        }

    }
}
