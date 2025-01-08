using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Country.Domain.Abstractions
{
    public class Result
    {
        public Result()
        {
            IsSuccess = true;
            Error = Error.None;
        }

        public Result(Error error)
        {
            Error = error;
            IsSuccess = false;
        }

        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; private set; }
        public static Result Success() => new();
        public static Result Failure(Error error) => new(error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        [JsonConstructor]
        public Result(TValue value): base()
        {
            _value = value;
        }

        public Result(Error error): base(error)
        {
        }

        [NotNull]
        public TValue Value => IsSuccess ? _value! :
            throw new InvalidOperationException("Value can not be accessed");

        public static implicit operator Result<TValue>(TValue value) => new(value);
        public static implicit operator Result<TValue>(Error error) => new(error);

        public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure) =>
            IsSuccess ? success(_value!) : failure(Error);
    }
}
