using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserDTO> RegisterAsync(RegisterDTO dto);
        Task<UserDTO> LoginAsync(LoginDTO dto);
    }
}
