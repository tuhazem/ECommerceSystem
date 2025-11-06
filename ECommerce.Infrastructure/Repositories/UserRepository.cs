using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbcontext;

        public UserRepository(AppDbContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        public async Task AddAsync(User user)
        {
            await dbcontext.Users.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbcontext.Users.FirstOrDefaultAsync(e => e.Email == email);
            
        }

        public async Task SaveChangesAsync()
        {
           await dbcontext.SaveChangesAsync();
        }
    }
}
