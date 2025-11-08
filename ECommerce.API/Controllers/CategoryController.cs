using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService service;

        public CategoryController(ICategoryService service)
        {
            this.service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() { 
        
            var category = await service.GetAllAsync();
            return Ok(category);
        
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) { 
        
            var category = await service.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto) {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var category = await service.AddAsync(dto);
            return Ok(category);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id , [FromBody] UpdateCategoryDTO dto) {
            var category = await service.UpdateAsync(id , dto);
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) { 
        
            await service.DeleteAsync(id);
            return NoContent();
        }

    }
}


