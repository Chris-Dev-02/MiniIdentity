namespace MiniIdentity.Enterprise.Models
{
    public class Role : Entity
    {
        public Role(string name, string description)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            Description = description;
            Created = DateTimeOffset.UtcNow;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }
        public string Description { get; private set; }

        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset? LastModified { get; private set; }

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

        protected override void Validate()
        {
            base.Validate();
        }

        private void Modify()
        {
            LastModified = DateTimeOffset.UtcNow;
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
