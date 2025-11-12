using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService service;

        public OrderController(IOrderService service)
        {
            this.service = service;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new KeyNotFoundException("UserId not found");

        [HttpPost]
        public async Task<IActionResult> PlaceOrder() {

            var userId = GetUserId();
            var oreder = await service.PlaceOrderAsync(userId);
            return Ok(oreder);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders() {

            var userId = GetUserId();
            var orders = await service.GetUserOrderAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId) {
        
            var userId = GetUserId();
            var order = await service.GetOrderByIdAsync(userId, orderId);
            return Ok(order);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{orderId:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status) { 
        
            await service.UpdateStatusAsync(orderId, status);
            return Ok(new { Message = "Order status updated successfully" });

        }
    }
}
