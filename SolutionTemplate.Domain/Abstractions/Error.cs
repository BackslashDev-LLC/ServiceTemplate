namespace SolutionTemplate.Domain.Abstractions
{
    /// <summary>
    /// An object which describes an application error
    /// </summary>
    /// <param name="Code">The code representing the error</param>
    /// <param name="Description">The description of the error</param>
    public record Error(string Code, string Description)
    {
        /// <summary>
        /// An instance of an error, with no additional detail
        /// </summary>
        public static Error None = new(string.Empty, string.Empty);
        /// <summary>
        /// An instance of an error, with a NULL value
        /// </summary>

        public static Error NullValue = new("Error.NullValue", "Null value was provided");
    }
}
