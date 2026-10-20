namespace MiniIdentity.Enterprise.Models
{
    //public class RefreshToken
    //{
    //    public string Id { get; private set; }
    //    public string UserId { get; private set; }

    //    public string Token { get; private set; }
    //    public string ReplacedByToken { get; private set; }

    //    public string CreatedByIp { get; private set; }
    //    public DateTime Created { get; private set; } = DateTime.Now;
    //    public DateTime ExpirationDate { get; set; }

    //    public DateTime? RevokedAt { get; private set; }


    //    public bool IsExpired => DateTime.UtcNow >= ExpirationDate;
    //    public bool IsRevoked => RevokedAt.HasValue;
    //    public bool IsActive => !IsExpired && !IsRevoked;

    //    public void Revoke()
    //    {
    //        RevokedAt = DateTime.UtcNow;
    //    }
    //}

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

        public string Token { get; private set; }
        public string? ReplacedByToken { get; private set; }

        public string CreatedByIp { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset ExpirationDate { get; private set; }

        public DateTimeOffset? RevokedAt { get; private set; }

        public User User { get; private set; }

        public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationDate;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => !IsExpired && !IsRevoked;

        protected override void Validate()
        {
            base.Validate();
        }

        public void Revoke(string? replacedByToken = null)
        {
            RevokedAt = DateTimeOffset.UtcNow;
            ReplacedByToken = replacedByToken;
        }
    }
}
