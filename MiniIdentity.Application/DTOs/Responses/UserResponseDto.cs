namespace MiniIdentity.Application.DTOs.Responses
{
    public class UserResponseDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool HasEmailBeenVerified { get; set; }
    }
}
