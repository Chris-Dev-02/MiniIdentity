namespace MiniIdentity.Application.DTOs.Requests
{
    public class ChangePasswordRequestDto
    {
        public string UserId {  get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
