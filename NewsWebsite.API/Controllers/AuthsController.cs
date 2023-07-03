using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.API.Dtos;
using NewsWebsite.API.Services;

namespace NewsWebsite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var Auth = await _authService.LoginAsync(loginDto.UserName, loginDto.Password);

            if(!Auth.IsAuthenticated)
                return NotFound(Auth.ErrorMessage);

            return Ok(Auth);
        }
    }
}
