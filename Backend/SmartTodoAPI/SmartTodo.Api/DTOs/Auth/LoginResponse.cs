namespace SmartTodoAPI.DTOs.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = "Bearer";

        public int ExpiresIn { get; set; }

        public UserResponse User { get; set; } = new(); //Instead of making another API call GET /profile
    }
}
