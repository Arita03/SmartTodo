using SmartTodoAPI.Models;

namespace SmartTodoAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email); // it may return null if it cant find any user existing with the given mailid

        Task<User> CreateUserAsync(User user); //Repository works with entities because it talks to the database.

        Task<bool> IsEmailExistsAsync(string email);
    }
}
