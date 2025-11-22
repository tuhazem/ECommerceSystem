using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
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
        private readonly AppDbContext dbContext;
        private readonly IProductRepository productrepo;

        public OrderService(IOrderRepository orderrepo ,
            ICartRepository cartrepo , IMapper mapper , 
            AppDbContext dbContext ,
            IProductRepository productrepo)
        {
            this.orderrepo = orderrepo;
            this.cartrepo = cartrepo;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.productrepo = productrepo;
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

            foreach (var item in cart.Items) 
            {
                var product = item.Product ?? await
                    productrepo.GetByIdAsync(item.ProductId);
                if(product == null)
                    throw new KeyNotFoundException($"{item.ProductId} not found");
                if (product.Stock < item.Quantity) {
                    throw new KeyNotFoundException($"product {product.Name} does Not have enough stock avaliable {product.Stock}");
                }
                    
            }


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
            using (var tx = await
                dbContext.Database.BeginTransactionAsync())
            {

                try
                {

                    await orderrepo.AddAsync(oreder);
                    foreach (var cartitem in cart.Items)
                    {
                        var product = cartitem.Product ?? await productrepo.GetByIdAsync(cartitem.ProductId);
                        if (product == null) throw new KeyNotFoundException($"Product(id = {cartitem.ProductId}) not found");
                        product.Stock -= cartitem.Quantity;
                        if (product.Stock < 0)
                            throw new InvalidOperationException($"Product {product.Name} Not enough in stock");
                        await productrepo.UpdateAsync(product);
                    }
                    await cartrepo.ClearCartAsync(UserId);
                    await tx.CommitAsync();
                }
                catch {
                    await tx.RollbackAsync();
                    throw;
                }
            
            
            }
                //await orderrepo.AddAsync(oreder);
                //await cartrepo.DeleteAsync(cart);
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
