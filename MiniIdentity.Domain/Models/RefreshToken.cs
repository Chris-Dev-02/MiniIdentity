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

    public class RefreshToken
    {
        public RefreshToken(
            string userId,
            string token,
            DateTime expirationDate,
            string createdByIp)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Token = token;
            ExpirationDate = expirationDate;
            CreatedByIp = createdByIp;
            Created = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string UserId { get; private set; }

        public string Token { get; private set; }
        public string? ReplacedByToken { get; private set; }

        public string CreatedByIp { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public DateTime? RevokedAt { get; private set; }

        public User User { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpirationDate;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => !IsExpired && !IsRevoked;

        public void Revoke(string? replacedByToken = null)
        {
            RevokedAt = DateTime.UtcNow;
            ReplacedByToken = replacedByToken;
        }
    }
}
