using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService auth;

        public AuthController(IAuthService auth)
        {
            this.auth = auth;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDTO dto) { 
        
            var result = await auth.RegisterAsync(dto);
            return Ok(result);

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto) { 
        
            var result = await auth.LoginAsync(dto);
            return Ok(result);

        }

    }
}
