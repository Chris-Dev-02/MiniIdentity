namespace MiniIdentity.Enterprise.Models
{
    //public class User
    //{
    //    public User(string name, string email, string password, string salt)
    //    {
    //        Id = Guid.NewGuid().ToString();
    //        Username = name;
    //        NormalizedUsername = name.ToUpperInvariant();
    //        Email = email;
    //        NormalizedEmail = email.ToUpperInvariant();
    //        PasswordHash = password;
    //        Salt = salt;
    //    }

    //    public string Id { get; set; }
    //    public string Username { get; set; }
    //    public string NormalizedUsername { get; private set; }
    //    public string Email { get; set; }
    //    public string NormalizedEmail { get; private set; }
    //    public string PasswordHash { get; private set; }
    //    public string Salt { get; private set; }

    //    public bool HasEmailBeenVerified { get; private set; } = false;

    //    public bool IsBlocked { get; private set; } = false;
    //    public bool IsActive { get; private set; } = true;

    //    public int FailedLoginAttempts { get; private set; }
    //    public DateTime? LockoutEnd { get; private set; }


    //    public DateTime Created { get; private set; } = DateTime.UtcNow;
    //    public DateTime LastModified { get; private set; }
    //    public DateTime? LastLogin { get; private set; }


    //    protected void Modify()
    //    {
    //        LastModified = DateTime.UtcNow;
    //    } 

    //    public void SetLoginDate()
    //    {
    //        LastLogin = DateTime.UtcNow;
    //    }

    //    public void Block()
    //    {
    //        IsBlocked = true;
    //        Modify();
    //    }

    //    public void Unblock()
    //    {
    //        IsBlocked = false;
    //        Modify();
    //    }

    //    public void Activate()
    //    {
    //        IsActive = true;
    //        Modify();
    //    }

    //    public void Deactivate()
    //    {
    //        IsActive = false;
    //        Modify();
    //    }

    //    public void ChangeUsername(string newUsername)
    //    {
    //        Username = newUsername;
    //        NormalizedUsername = newUsername.ToUpperInvariant();
    //        Modify();
    //    }

    //    public void ChangeEMail(string newEmail)
    //    {
    //        Email = newEmail;
    //        NormalizedEmail = newEmail.ToUpperInvariant();
    //        Modify();
    //    }

    //    public void ChangePassword(string newPassword)
    //    {
    //        PasswordHash = newPassword;
    //        Modify();
    //    }

    //    public void VerifiedEmail()
    //    {
    //        HasEmailBeenVerified = true;
    //        Modify();
    //    }

    //    public bool GetIsActive() => IsActive;

    //    public bool GetIsBlocked() => IsBlocked;
    //}

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
        public string NormalizedUsername { get; private set; }

        public string Email { get; private set; }
        public string NormalizedEmail { get; private set; }

        public string PasswordHash { get; private set; }
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

        private void Modify()
        {
            LastModified = DateTimeOffset.UtcNow;
        }

        public void SetLoginDate()
        {
            LastLogin = DateTimeOffset.UtcNow;
            Modify();
        }

        public void Block()
        {
            IsBlocked = true;
            Modify();
        }

        public void Unblock()
        {
            IsBlocked = false;
            FailedLoginAttempts = 0;
            LockoutEnd = null;
            Modify();
        }

        public void Activate()
        {
            IsActive = true;
            Modify();
        }

        public void Deactivate()
        {
            IsActive = false;
            Modify();
        }

        public void ChangeUsername(string newUsername)
        {
            Username = newUsername;
            NormalizedUsername = newUsername.ToUpperInvariant();
            Modify();
        }

        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
            NormalizedEmail = newEmail.ToUpperInvariant();
            Modify();
        }

        public void ChangePassword(string passwordHash, string salt)
        {
            PasswordHash = passwordHash;
            Salt = salt;
            Modify();
        }

        public void VerifyEmail()
        {
            HasEmailBeenVerified = true;
            Modify();
        }
    }
}
