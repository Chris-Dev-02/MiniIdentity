using HopperSoftware.Exceptions.Exceptions;
using MiniIdentity.Enterprise.Exceptions;

namespace MiniIdentity.Enterprise.Models
{
    /// <summary>
    /// Represents a role within the system, including its metadata and associated users.
    /// </summary>
    /// <remarks>A role is used to define a set of permissions or responsibilities that can be assigned to
    /// users. Each role has a unique identifier, a name, a normalized name for case-insensitive comparisons,  and an
    /// optional description. The role also tracks its creation and modification timestamps.</remarks>
    public class Role : Entity
    {
        public Role(string name, string description)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            Description = description;
            Created = DateTimeOffset.UtcNow;

            Validate();
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        /// <summary>
        /// Represents the normalized version of the name, typically used for comparison or lookup purposes.
        /// </summary>
        public string NormalizedName { get; private set; }
        /// <summary>
        /// Represents the description associated with the current instance.
        /// </summary>
        public string Description { get; private set; }

        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset? LastModified { get; private set; }

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

        protected override void Validate()
        {
            base.Validate();

            if (string.IsNullOrEmpty(Name))
                throw new ClientException(Errors.RequiredField, Modules.Role, "Name is required");

            if (Name.Length > GlobalValues.ShortField)
                throw new ClientException(Errors.InvalidSize, Modules.Role, $"Name must not exceed {GlobalValues.ShortField} characters");

            if (string.IsNullOrEmpty(Description))
                throw new ClientException(Errors.RequiredField, Modules.Role, "Description is required");

            if (Description.Length > GlobalValues.LongField)
                throw new ClientException(Errors.InvalidSize, Modules.Role, $"Description must not exceed {GlobalValues.LongField} characters");

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
        /// Updates the metadata of the object, including its name and description.
        /// </summary>
        /// <remarks>The <paramref name="name"/> is normalized to uppercase and stored as
        /// <c>NormalizedName</c>. This method also triggers a modification action by calling the <c>Modify</c>
        /// method.</remarks>
        /// <param name="name">The new name to assign to the object. Cannot be null or empty.</param>
        /// <param name="description">The new description to assign to the object. Can be null or empty.</param>
        public void UpdateMetadata(string name, string description)
        {
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            Description = description;
            Modify();
        }
    }
}
