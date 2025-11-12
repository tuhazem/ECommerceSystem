using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> PlaceOrderAsync(string UserId);
        Task<IEnumerable<OrderDTO>> GetUserOrderAsync(string UserId);
        Task<OrderDTO> GetOrderByIdAsync(string UserId , int Id);
        Task UpdateStatusAsync(int orderId, string status);
    }
}
