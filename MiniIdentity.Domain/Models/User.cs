namespace MiniIdentity.Enterprise.Models
{
    /// <summary>
    /// Represents a user within the system, including authentication and account state information.
    /// </summary>
    /// <remarks>The <see cref="User"/> class encapsulates user-related data such as credentials, account
    /// status,  and roles. It provides methods to manage the user's state, including activation, blocking,  and
    /// updating credentials. Instances of this class are immutable in terms of identifiers  and timestamps, but allow
    /// controlled updates to other properties through provided methods.</remarks>
    public class User : Entity
    {
        public User(string username, string email, string passwordHash, string salt)
        {
            Id = Guid.NewGuid().ToString();

            Username = username;
            NormalizedUsername = username.ToUpperInvariant();

            Email = email;
            NormalizedEmail = email.ToUpperInvariant();

            PasswordHash = passwordHash;
            Salt = salt;

            Created = DateTimeOffset.UtcNow;
        }

        public string Id { get; private set; }

        public string Username { get; private set; }
        /// <summary>
        /// Represents the normalized version of the username, typically used for case-insensitive comparisons.
        /// </summary>
        public string NormalizedUsername { get; private set; }

        public string Email { get; private set; }
        /// <summary>
        /// Representss the normalized email address associated with the user.
        /// </summary>
        public string NormalizedEmail { get; private set; }

        /// <summary>
        /// Gets the hashed representation of the user's password.
        /// </summary>
        public string PasswordHash { get; private set; }
        /// <summary>
        /// Represents the cryptographic salt used for hashing operations.
        /// </summary>
        public string Salt { get; private set; }

        public bool HasEmailBeenVerified { get; private set; } = false;
        public bool IsBlocked { get; private set; } = false;
        public bool IsActive { get; private set; } = true;

        public int FailedLoginAttempts { get; private set; }
        public DateTimeOffset? LockoutEnd { get; private set; }

        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset? LastModified { get; private set; }
        public DateTimeOffset? LastLogin { get; private set; }

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
        public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

        protected override void Validate()
        {
            base.Validate();
        }

        /// <summary>
        /// Updates the <see cref="LastModified"/> property to the current UTC date and time.
        /// </summary>
        /// <remarks>This method sets the <see cref="LastModified"/> property to the current time in UTC
        /// format. It is intended to track the most recent modification timestamp.</remarks>
        private void Modify()
        {
            LastModified = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Updates the <see cref="LastLogin"/> property to the current UTC date and time.
        /// </summary>
        /// <remarks>This method sets the <see cref="LastLogin"/> property to the current date and time in
        /// UTC format and invokes the <see cref="Modify"/> method to apply additional changes. It is typically used to
        /// record the most recent login time for a user.</remarks>
        public void SetLoginDate()
        {
            LastLogin = DateTimeOffset.UtcNow;
            Modify();
        }

        /// <summary>
        /// Blocks the current instance, setting its state to indicate it is blocked.
        /// </summary>
        /// <remarks>This method sets the <see cref="IsBlocked"/> property to <see langword="true"/>  and
        /// performs additional modifications by invoking the <see cref="Modify"/> method.</remarks>
        public void Block()
        {
            IsBlocked = true;
            Modify();
        }

        /// <summary>
        /// Removes the block on the account, resetting the lockout state and failed login attempts.
        /// </summary>
        /// <remarks>This method clears the lockout state by setting <see cref="IsBlocked"/> to <see
        /// langword="false"/>, resetting <see cref="FailedLoginAttempts"/> to zero, and nullifying the <see
        /// cref="LockoutEnd"/> timestamp. It also invokes the <see cref="Modify"/> method to persist the
        /// changes.</remarks>
        public void Unblock()
        {
            IsBlocked = false;
            FailedLoginAttempts = 0;
            LockoutEnd = null;
            Modify();
        }

        /// <summary>
        /// Activates the current instance, setting it to an active state.
        /// </summary>
        /// <remarks>This method sets the <see cref="IsActive"/> property to <see langword="true"/>  and
        /// performs additional modifications by invoking the <see cref="Modify"/> method.</remarks>
        public void Activate()
        {
            IsActive = true;
            Modify();
        }

        /// <summary>
        /// Deactivates the current instance, marking it as inactive.
        /// </summary>
        /// <remarks>This method sets the <see cref="IsActive"/> property to <see langword="false"/>  and
        /// performs any additional modifications required to reflect the deactivation.</remarks>
        public void Deactivate()
        {
            IsActive = false;
            Modify();
        }

        /// <summary>
        /// Changes the username of the user and updates the normalized username.
        /// </summary>
        /// <remarks>This method updates both the <see cref="Username"/> and <see
        /// cref="NormalizedUsername"/> properties. The normalized username is automatically converted to uppercase
        /// using invariant culture.</remarks>
        /// <param name="newUsername">The new username to assign. Cannot be null or empty.</param>
        public void ChangeUsername(string newUsername)
        {
            Username = newUsername;
            NormalizedUsername = newUsername.ToUpperInvariant();
            Modify();
        }

        /// <summary>
        /// Updates the email address associated with the user.
        /// </summary>
        /// <remarks>This method updates both the email and its normalized form. Ensure that the provided
        /// email address is valid and properly formatted before calling this method.</remarks>
        /// <param name="newEmail">The new email address to assign to the user. Cannot be null or empty.</param>
        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
            NormalizedEmail = newEmail.ToUpperInvariant();
            Modify();
        }

        /// <summary>
        /// Updates the user's password hash and salt values.
        /// </summary>
        /// <remarks>This method updates the user's password by setting the provided hash and salt values.
        /// Ensure that the <paramref name="passwordHash"/> and <paramref name="salt"/> are securely generated and
        /// validated before calling this method.</remarks>
        /// <param name="passwordHash">The new hashed password. Cannot be null or empty.</param>
        /// <param name="salt">The cryptographic salt associated with the new password hash. Cannot be null or empty.</param>
        public void ChangePassword(string passwordHash, string salt)
        {
            PasswordHash = passwordHash;
            Salt = salt;
            Modify();
        }

        /// <summary>
        /// Marks the email as verified and updates the associated state.
        /// </summary>
        /// <remarks>This method sets the <see cref="HasEmailBeenVerified"/> property to <see
        /// langword="true"/>  and performs any additional modifications required to reflect the verification.</remarks>
        public void VerifyEmail()
        {
            HasEmailBeenVerified = true;
            Modify();
        }
    }
}
