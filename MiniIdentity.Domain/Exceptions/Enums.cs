namespace MiniIdentity.Enterprise.Exceptions
{
    public enum Modules
    {
        User = 0,
        Role = 1,
        RefreshToken = 2
    }

    public enum Errors
    {
        RequiredField = 0,
        InvalidFieldValue = 1,
        InvalidSize = 2,
        InvalidOperation = 3,

        EntityNotFound = 4,
        DuplicateEntity = 5,

        InvalidHierarchy = 6,
        InvalidParent = 7,

        AlreadyActive = 8,
        AlreadyInactive = 9,
        InvalidState = 10
    }
}
