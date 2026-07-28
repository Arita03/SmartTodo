using Microsoft.EntityFrameworkCore;
using SmartTodoAPI.Data;
using SmartTodoAPI.Models;
using System.Linq;
using SmartTodoAPI.Repositories.Interfaces;

namespace SmartTodoAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;    
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(x => x.Email == email);
        }
    }
}
