using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<IEnumerable<Order>> GetAllByUserAsync(string UserId);
        Task<Order?> GetByIdAsync(int Id , string UserId);
    }
}
