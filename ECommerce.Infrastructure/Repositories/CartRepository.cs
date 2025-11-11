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
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext dbcontext;

        public CartRepository(AppDbContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            dbcontext.Carts.Add(cart);
            //cart.Id = 0;
            await dbcontext.SaveChangesAsync();
            return cart;
        }

        public async Task DeleteAsync(Cart cart)
        {
            dbcontext.Carts.Remove(cart);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<Cart?> GetByCartIdAsync(int cartId)
        {
            return await dbcontext.Carts.Include(c => c.Items).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(a=> a.Id == cartId);
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await dbcontext.Carts.Include(c=> c.Items)
                .ThenInclude(p=> p.Product).FirstOrDefaultAsync(a=> a.UserId == userId);
        }

        public async Task<Cart> UpdateAsync(Cart cart)
        {
            dbcontext.Carts.Update(cart);
            await dbcontext.SaveChangesAsync();
            return cart;
        }
    }
}
