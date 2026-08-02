namespace SmartTodoAPI.DTOs.Auth
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty; //You might prefer the initial value for the FirstName property to be the empty string rather than null
        public string Password { get; set; } = string.Empty;
    }
}
