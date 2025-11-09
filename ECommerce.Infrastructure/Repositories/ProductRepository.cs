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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext dbcontext;

        public ProductRepository(AppDbContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<Product?> AddAsync(Product product)
        {
            dbcontext.Products.Add(product);
            await dbcontext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await dbcontext.Products.FindAsync(id);
            if (product != null) { 
                dbcontext.Products.Remove(product);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await dbcontext.Products.Include(a=> a.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await dbcontext.Products
                .Include(c => c.Category)
                .FirstOrDefaultAsync(a=> a.Id == id) ?? throw new KeyNotFoundException("Product No Found") ;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            dbcontext.Products.Update(product);
            await dbcontext.SaveChangesAsync();
            return product;
        }
    }
}
