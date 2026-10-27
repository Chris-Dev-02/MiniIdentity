using HopperSoftware.Exceptions.Exceptions;
using MiniIdentity.Enterprise.Exceptions;

namespace MiniIdentity.Enterprise.Models
{
    /// <summary>
    /// Represents the association between a user and a role within a system.
    /// </summary>
    /// <remarks>This class encapsulates the relationship between a user and a role, including the user ID,
    /// role ID,  and the timestamp indicating when the association was created. Instances of this class are immutable 
    /// after initialization.</remarks>
    public class UserRole : Entity
    {
        public UserRole(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
            Created = DateTimeOffset.UtcNow;

            Validate();
        }

        public string UserId { get; private set; }
        public string RoleId { get; private set; }
        public DateTimeOffset Created { get; private set; }

        protected override void Validate()
        {
            base.Validate();

            if (string.IsNullOrEmpty(UserId))
                throw new ClientException(Errors.RequiredField, Modules.UserRole, "UserId is required");

            if (UserId.Length > GlobalValues.KeyLength)
                throw new ClientException(Errors.InvalidSize, Modules.UserRole, $"UserId must not exceed {GlobalValues.KeyLength} characters");

            if (string.IsNullOrEmpty(RoleId)) 
                throw new ClientException(Errors.RequiredField, Modules.UserRole, "RoleId is required");

            if (RoleId.Length > GlobalValues.KeyLength) 
                throw new ClientException(Errors.InvalidSize, Modules.UserRole, $"RoleId must not exceed {GlobalValues.KeyLength} characters");
        }
    }
}
