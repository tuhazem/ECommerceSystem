using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext dbcontext;

        public OrderRepository(AppDbContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<Order> AddAsync(Order order)
        {
            dbcontext.Orders.Add(order);
            await dbcontext.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllByUserAsync(string UserId)
        {
            return await dbcontext.Orders.
                Include(o => o.Items).ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == UserId).OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int Id, string UserId)
        {
            return await dbcontext.Orders
                .Include(o => o.Items).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == Id && o.UserId == UserId);
        }


        public async Task UpdateStatusAsync(int orderId, string newStatus)
        {
            var order = await dbcontext.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            order.Status = newStatus;
            await dbcontext.SaveChangesAsync();
        }

    }
}
