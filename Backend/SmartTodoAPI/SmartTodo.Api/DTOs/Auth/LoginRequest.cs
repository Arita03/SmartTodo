using System.ComponentModel.DataAnnotations;

namespace SmartTodoAPI.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; //You might prefer the initial value for the FirstName property to be the empty string rather than null

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
