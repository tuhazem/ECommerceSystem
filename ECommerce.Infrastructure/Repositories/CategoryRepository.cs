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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext dbcontext;

        public CategoryRepository(AppDbContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task<Category> AddAsync(Category category)
        {
            dbcontext.Categories.Add(category);
            await dbcontext.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(int id)
        {
            var res = await dbcontext.Categories.FindAsync(id);
            if (res != null) {
                dbcontext.Categories.Remove(res);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbcontext.Categories.Include(a => a.Products).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await dbcontext.Categories.Include(c => c.Products).FirstOrDefaultAsync(a=> a.Id == id);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            dbcontext.Categories.Update(category);
            await dbcontext.SaveChangesAsync();
            return category;
        }
    }
}
