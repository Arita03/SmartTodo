using SmartTodoAPI.DTOs.Auth;

namespace SmartTodoAPI.Services.Interfaces
{
    public interface IUserService
    {
    
        Task<UserResponse> RegisterUserAsync(RegisterUserRequest request); //Service works with DTOs because it represents the application's business operations.
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
