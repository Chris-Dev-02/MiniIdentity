namespace MiniIdentity.Enterprise.Models
{
    public class UserRole
    {
        public UserRole(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
            Created = DateTimeOffset.UtcNow;
        }

        public string UserId { get; private set; }
        public string RoleId { get; private set; }
        public DateTimeOffset Created { get; private set; }
    }
}
