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
    public class CartService : ICartService
    {
        private readonly ICartRepository cartrepository;
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;

        public CartService(ICartRepository cartrepository 
            , IMapper mapper , 
            IProductRepository productRepository)
        {
            this.cartrepository = cartrepository;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }


        public async Task<CartDTO> AddItemAsync(string userId, AddCartItemDTO dto)
        {
            var cart = await cartrepository.GetByUserIdAsync(userId) ??
                await cartrepository.CreateAsync(new Domain.Entities.Cart { UserId = userId });

            var product = await productRepository.GetByIdAsync(dto.ProductId);
            if (product == null) throw new KeyNotFoundException("Product Not Found");

            var existing = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
            }
            else {

                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = dto.Quantity,
                    UntiPrice = product.Price
                });

            }
                await cartrepository.UpdateAsync(cart);
                return mapper.Map<CartDTO>(cart);

        }


        public async Task DeleteItemAsync(string userId, int cartitemId)
        {
            var cart = await cartrepository.GetByUserIdAsync(userId) ??
                throw new KeyNotFoundException("Cart Not Foound");

            var item = cart.Items.FirstOrDefault(a=> a.Id == cartitemId);
            if (item == null) return;
            cart.Items.Remove(item);
            await cartrepository.UpdateAsync(cart);
        }

        public async Task<CartDTO> GetCartForCurrentUserAsync(string userId)
        {
            var cart = await cartrepository.GetByUserIdAsync(userId);
            if (cart == null) {

                cart = new Cart { UserId = userId };
                cart = await cartrepository.CreateAsync(cart);
            }
            return mapper.Map<CartDTO>(cart);
        }

        public async Task RemoveCartAsync(string userId)
        {
            var cart = await cartrepository.GetByUserIdAsync(userId) ??
                throw new KeyNotFoundException("Cart Not Found");

            cart.Items.Clear();
            await cartrepository.UpdateAsync(cart);
        }


        public async Task<CartDTO> UpdateItemAsync(string userId, UpdateCartItemDTO dto)
        {
            var cart = await cartrepository.GetByUserIdAsync(userId) ??
                throw new KeyNotFoundException("Cart Not Found");

            var item = cart.Items.FirstOrDefault(i=> i.Id == dto.CartItemId);
            if (item == null) throw new KeyNotFoundException("Item Not Found");
            if (dto.Quantity <= 0)
            {

                cart.Items.Remove(item);
            }
            else { 
            
                item.Quantity = dto.Quantity;
            }
            await cartrepository.UpdateAsync(cart);
            return mapper.Map<CartDTO>(cart);

        }
    }
}
