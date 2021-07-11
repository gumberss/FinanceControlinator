using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.Utils
{
    [Serializable]
    public struct Result<T, E> where E : class
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        private readonly E _error;
        public E Error => _error;

        private readonly T _value;
        public T Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("Result.Value can't be used when an error happened, plese, check IsSuccess or IsFailure property before use Value property");

        public Result(T value)
        {
            IsFailure = false;
            _value = value;
            _error = default;
        }

        public Result(E error)
        {
            IsFailure = error != default;
            _value = default;
            _error = error;
        }

        public static implicit operator Result<T, E>(T value)
            => new Result<T, E>(value);

        public static implicit operator Result<T, E>(E error)
            => new Result<T, E>(error);

        public static implicit operator T(Result<T, E> result)
            => result.Value;

        public static implicit operator E(Result<T, E> result)
            => result.Error;
    }

    public class Result
    {
        public static async Task<Result<T, Exception>> Try<T, EType>(Task<T> func) where EType : Exception
        {
            Func<Task<Result<T, Exception>>> tryFunction = async () =>
            {
                try
                {
                    return await func;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            };

            return await Task.Run(() => tryFunction.Invoke());
        }
    }
}
