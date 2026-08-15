using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IJwtService _jwtService;
        

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
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
        public async Task <LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            string token;
           if (user == null)
           {
                throw new NotFoundException("No account found with this email address.");
           }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }
            token = _jwtService.GenerateToken(user);
         
            var response = new LoginResponse()
            {
                AccessToken = token,
                TokenType="Bearer",
                ExpiresIn = 3600,
                User = new UserResponse() { 
                    Id= user.Id,    
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    Email = user.Email,

                }
            };
            return response;
        }
    }
}
