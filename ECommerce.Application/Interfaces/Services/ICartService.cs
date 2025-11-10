using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDTO> GetCartForCurrentUserAsync(string userId);
        Task<CartDTO> AddItemAsync(string userId , AddCartItemDTO dto);
        Task<CartDTO> UpdateItemAsync(string userId , UpdateCartItemDTO dto);
        Task DeleteItemAsync(string userId , int cartitemId);
        Task RemoveCartAsync(string userId);
    }
}
