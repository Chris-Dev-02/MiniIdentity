namespace MiniIdentity.Enterprise.Models
{
    /// <summary>
    /// Represents the association between a user and a role within a system.
    /// </summary>
    /// <remarks>This class encapsulates the relationship between a user and a role, including the user ID,
    /// role ID,  and the timestamp indicating when the association was created. Instances of this class are immutable 
    /// after initialization.</remarks>
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
