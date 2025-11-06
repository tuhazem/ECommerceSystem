using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> role;
        private readonly IMapper mapper;
        private readonly IConfiguration config;

        public AuthService(UserManager<User> userManager , RoleManager<IdentityRole> role, IMapper mapper , IConfiguration config)
        {
            this.userManager = userManager;
            this.role = role;
            this.mapper = mapper;
            this.config = config;
        }

        public async Task<UserDTO> LoginAsync(LoginDTO dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid credentials");

            return await GenerateTokenAsync(user);
        }


        public async Task<UserDTO> RegisterAsync(RegisterDTO dto)
        {
            var exist = await userManager.FindByEmailAsync(dto.Email);
            if (exist != null) throw new Exception("Email is already Registred");

            var  user = mapper.Map<User>(dto);
            user.EmailConfirmed = true;

            var res = await userManager.CreateAsync(user, dto.Password);
            if (!res.Succeeded)
            {
                throw new Exception(string.Join("; ", res.Errors.Select(e => e.Description)));
            }

            var rolename = "Customer";
            if (!await role.RoleExistsAsync(rolename)) await role.CreateAsync(new IdentityRole(rolename));

            await userManager.AddToRoleAsync(user, rolename);
            return await GenerateTokenAsync(user);
        }

        private async Task<UserDTO> GenerateTokenAsync(User user)
        {

            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim> { 
            
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(ClaimTypes.Email , user.Email ?? ""),
                new Claim(ClaimTypes.Name , user.FullName)

            };

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );

            return new UserDTO
            {
                Name = user.FullName,
                Email = user.Email ?? "",
                Role = user.Role,
                Token = new JwtSecurityTokenHandler().WriteToken(token)

            };

        }


    }
}
