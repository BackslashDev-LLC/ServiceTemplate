namespace SolutionTemplate.Domain.Abstractions
{
    /// <summary>
    /// An object used to indicate the result (Success or Failure) of an operation
    /// </summary>
    public class Result
    {
        internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Indicates that the operation succeeded
        /// </summary>
        public bool IsSuccess { get; }
        /// <summary>
        /// Inidcates that the operation failed
        /// </summary>

        public bool IsFailure => !IsSuccess;
        /// <summary>
        /// An object representing the operation Error, if present
        /// </summary>

        public Error Error { get; }
        /// <summary>
        /// Used to indicate a successful operation
        /// </summary>
        /// <returns></returns>

        public static Result Success() => new(true, Error.None);
        /// <summary>
        /// Used to indicated a failed operation
        /// </summary>
        /// <param name="error">The error that caused the failure</param>
        /// <returns></returns>

        public static Result Failure(Error error) => new(false, error);
        /// <summary>
        /// Used to indicate a successful operation, and return a value
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        /// <summary>
        /// Used to indicate a failed operation
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="error">The error that caused the failure</param>
        /// <returns></returns>

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        internal static Result<TValue> Create<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }

    /// <summary>
    /// An override of Result which includes an object returned from an operation
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        /// <summary>
        /// The object returned from the operation
        /// </summary>
        public TValue? Value => IsSuccess
            ? _value!
            : default;

        /// <summary>
        /// A default create method
        /// </summary>
        /// <param name="value"></param>

        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }
}
