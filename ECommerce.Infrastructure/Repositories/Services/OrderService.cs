using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderrepo;
        private readonly ICartRepository cartrepo;
        private readonly IMapper mapper;

        public OrderService(IOrderRepository orderrepo ,
            ICartRepository cartrepo , IMapper mapper)
        {
            this.orderrepo = orderrepo;
            this.cartrepo = cartrepo;
            this.mapper = mapper;
        }

        public async Task<OrderDTO> GetOrderByIdAsync(string UserId, int Id)
        {
            var oreder = await orderrepo.GetByIdAsync(Id, UserId);
            if (oreder == null)
                throw new KeyNotFoundException("Order not found");
            return mapper.Map<OrderDTO>(oreder);
        }

        public async Task<IEnumerable<OrderDTO>> GetUserOrderAsync(string UserId)
        {
            var orders = await orderrepo.GetAllByUserAsync(UserId);
            return mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> PlaceOrderAsync(string UserId)
        {
            var cart = await cartrepo.GetByUserIdAsync(UserId);
            if (cart == null || !cart.Items.Any())
                throw new KeyNotFoundException("Cart is Empty");


            var oreder = new Order
            {
                UserId = UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.Items.Sum(i => i.Quantity * i.Product!.Price),
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product!.Price
                }).ToList()

            };
           await orderrepo.AddAsync(oreder);
           await cartrepo.DeleteAsync(cart);
            return mapper.Map<OrderDTO>(oreder);
        }

        public async Task UpdateStatusAsync(int orderId, string status) {
        
            var order = await orderrepo.GetByIdAsync(orderId,"");
            if (order == null)
                throw new KeyNotFoundException("Order not found");
            order.Status = status;
            await orderrepo.UpdateStatusAsync(orderId, status);


        }

    }
}
