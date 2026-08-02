using SmartTodoAPI.DTOs.Auth;
using SmartTodoAPI.Exceptions;
using SmartTodoAPI.Models;
using SmartTodoAPI.Repositories.Interfaces;
using SmartTodoAPI.Services.Interfaces;

namespace SmartTodoAPI.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            var emailExists = await _userRepository.IsEmailExistsAsync(request.Email);
            if (emailExists)
            {
                throw new BadRequestException("Email already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hashedPassword
            };

            var createdUser = await _userRepository.CreateUserAsync(user);

            return new UserResponse
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email
            };
        }
    }
}
