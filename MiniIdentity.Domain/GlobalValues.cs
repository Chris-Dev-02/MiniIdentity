namespace MiniIdentity.Enterprise
{
    public static class GlobalValues
    {
        /// <summary>
        /// Represents the fixed length of a key in characters.
        /// </summary>
        /// <remarks>This constant defines the maximum allowable length for keys used in the application.
        /// It is intended to ensure consistency and prevent errors related to key length.</remarks>
        public const int KeyLength = 50;

        /// <summary>
        /// Represents a constant value of 100.
        /// </summary>
        /// <remarks>This field is intended for use in scenarios where a predefined constant value of 100
        /// is required.</remarks>
        public const int ShortField = 100;

        /// <summary>
        /// Represents the medium field value used for categorization or configuration purposes.
        /// </summary>
        /// <remarks>This constant is typically used to define a medium-sized threshold or limit in
        /// various contexts.</remarks>
        public const int MediumField = 200;

        /// <summary>
        /// Represents a constant value used to define a long field length.
        /// </summary>
        /// <remarks>This constant can be used in scenarios where a predefined field length of 500 is
        /// required.</remarks>
        public const int LongField = 500;
    }
}
