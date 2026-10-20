namespace MiniIdentity.Enterprise.Models
{
    /// <summary>
    /// Represents a refresh token used for user authentication and session management.
    /// </summary>
    /// <remarks>A refresh token is a secure token issued to a user that can be used to obtain a new access
    /// token  without requiring the user to reauthenticate. This class provides properties to track the token's 
    /// lifecycle, including its creation, expiration, and revocation status.   The token is associated with a specific
    /// user and includes metadata such as the IP address from  which it was created. It also supports token replacement
    /// scenarios, allowing a revoked token to  reference the token that replaced it.</remarks>
    public class RefreshToken : Entity
    {
        public RefreshToken(
            string userId,
            string token,
            DateTimeOffset expirationDate,
            string createdByIp)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Token = token;
            ExpirationDate = expirationDate;
            CreatedByIp = createdByIp;
            Created = DateTimeOffset.UtcNow;
        }

        public string Id { get; private set; }
        public string UserId { get; private set; }

        /// <summary>
        /// Represents the refresh token used to refresh session.
        /// </summary>
        public string Token { get; private set; }
        /// <summary>
        /// Represents the token that replaces the current one, if applicable.
        /// </summary>
        public string? ReplacedByToken { get; private set; }

        /// <summary>
        /// Represents the IP address of the user who created the entity.
        /// </summary>
        public string CreatedByIp { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset ExpirationDate { get; private set; }

        public DateTimeOffset? RevokedAt { get; private set; }

        public User User { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current instance has expired.
        /// </summary>
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationDate;
        /// <summary>
        /// Gets a value indicating whether the entity has been revoked.
        /// </summary>
        public bool IsRevoked => RevokedAt.HasValue;
        /// <summary>
        /// Gets a value indicating whether the entity is currently active.
        /// </summary>
        /// <remarks>An entity is considered active if it is neither expired nor revoked.</remarks>
        public bool IsActive => !IsExpired && !IsRevoked;

        protected override void Validate()
        {
            base.Validate();
        }

        /// <summary>
        /// Revokes the current token and optionally specifies a replacement token.
        /// </summary>
        /// <remarks>This method sets the revocation timestamp to the current UTC time and updates the
        /// replacement token, if provided.</remarks>
        /// <param name="replacedByToken">The token that replaces the current token, or <see langword="null"/> if no replacement token is specified.</param>
        public void Revoke(string? replacedByToken = null)
        {
            RevokedAt = DateTimeOffset.UtcNow;
            ReplacedByToken = replacedByToken;
        }
    }
}
