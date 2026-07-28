using Microsoft.AspNetCore.Mvc;
using SmartTodoAPI.DTOs.Auth;
using SmartTodoAPI.Responses;
using SmartTodoAPI.Services.Interfaces;

namespace SmartTodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var response = await _userService.RegisterUserAsync(request);

            return Ok(new ApiResponse<UserResponse>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = response
            });
        }
    }
}
