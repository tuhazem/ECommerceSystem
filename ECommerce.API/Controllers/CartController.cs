using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService service;

        public CartController(ICartService service)
        {
            this.service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("UserId not found");

        [HttpGet]
        public async Task<IActionResult> GetCart() {

            var userId = GetUserId();
            var cart = await service.GetCartForCurrentUserAsync(userId);
            return Ok(cart);

        }

        [HttpPost("Items")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemDTO dto) {

            var userId = GetUserId();
            var cart = await service.AddItemAsync(userId, dto);
            return Ok(cart);
        }

        [HttpPut("Items")]
        public async Task<IActionResult> Updateitem([FromBody] UpdateCartItemDTO dto) { 
        
            var userId = GetUserId();
            var cart = await service.UpdateItemAsync(userId, dto);
            return Ok(cart);
        }

        [HttpDelete("Item/{cartItemId:int}")]
        public async Task<IActionResult> DeleteItem(int cartItemId) {

            var userId = GetUserId();
            await service.DeleteItemAsync(userId, cartItemId);
            return NoContent();
        }

        [HttpDelete("ClearCart")]
        public async Task<IActionResult> Clear() { 
        
            var userId = GetUserId();
            await service.RemoveCartAsync(userId);
            return NoContent();
        }


    }
}
