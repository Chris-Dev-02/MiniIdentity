namespace MiniIdentity.Enterprise.Models
{
    /// <summary>
    /// Represents the base class for entities, providing a foundation for validation and other common functionality.
    /// </summary>
    /// <remarks>This class is intended to be inherited by other entity classes. Derived classes can override
    /// the <see cref="Validate"/> method to implement custom validation logic.</remarks>
    public abstract class Entity
    {
        /// <summary>
        /// 
        /// </summary>
        protected virtual void Validate()
        {

        }
    }
}
