namespace MiniIdentity.Application.DTOs.Responses
{
    public class PasswordHashResult
    {
        public string Hash { get; set; }

        public string Salt { get; set; }
    }
}
