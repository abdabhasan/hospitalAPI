using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Auth.Login;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalPresentation.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly clsUser _userService;
        private readonly TokenHelper _tokenHelper;


        public AuthController(clsUser userService, TokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            bool IsValidCredentials = await _userService.IsValidCredentialsAsync(model.Username, model.Password);

            if (IsValidCredentials)
            {
                UserDTO user = await _userService.GetUserByUsernameAsync(model.Username);

                var token = _tokenHelper.GenerateToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }
    }
}