using SmartTodoAPI.DTOs.Auth;
using SmartTodoAPI.Models;

namespace SmartTodoAPI.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
