namespace MiniIdentity.Enterprise.Models
{
    //public class Role
    //{
    //    public Role(string name, string description)
    //    {
    //        Id = Guid.NewGuid().ToString();
    //        Name = name;
    //        Description = description;
    //        NormalizedName = Name.ToUpperInvariant();
    //    }

    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public string NormalizedName { get; private set; }
    //    public string Description { get; set; }
    //    public DateTime Created { get; set; } = DateTime.UtcNow;
    //    public DateTime LastModified { get; set; }

    //    public void Modify()
    //    {
    //        LastModified = DateTime.UtcNow;
    //    }

    //    public void UpdateMetadata(string name, string description)
    //    {
    //        Name = name;
    //        NormalizedName = Name.ToUpperInvariant();
    //        Description = description;

    //        Modify();
    //    } 
    //}

    public class Role
    {
        public Role(string name, string description)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            Description = description;
            Created = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }
        public string Description { get; private set; }

        public DateTime Created { get; private set; }
        public DateTime? LastModified { get; private set; }

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

        private void Modify()
        {
            LastModified = DateTime.UtcNow;
        }

        public void UpdateMetadata(string name, string description)
        {
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            Description = description;
            Modify();
        }
    }
}
