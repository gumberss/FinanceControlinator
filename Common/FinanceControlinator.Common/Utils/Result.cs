using FinanceControlinator.Common.Exceptions;
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
        public static async Task<Result<T, BusinessException>> Try<T>(Task<T> func)
        {
            Func<Task<Result<T, BusinessException>>> tryFunction = async () =>
            {
                try
                {
                    return await func;
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            };

            return await Task.Run(() => tryFunction.Invoke());
        }

        public static async Task<Result<T, BusinessException>> Try<T>(Func<Task<T>> func)
        {
            Func<Task<Result<T, BusinessException>>> tryFunction = async () =>
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            };

            return await Task.Run(() => tryFunction.Invoke());
        }

        public static async Task<Result<T, BusinessException>> Try<T>(Func<T> func)
        {
            Func<Result<T, BusinessException>> tryFunction = () =>
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            };

            return await Task.Run(() => tryFunction.Invoke());
        }

        public static async Task<Result<bool, BusinessException>> Try(Task func)
        {
            Func<Task<Result<bool, BusinessException>>> tryFunction = async () =>
            {
                try
                {
                    await func;
                    return true;
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            };

            return await Task.Run(() => tryFunction.Invoke());
        }

        public static async Task<Result<bool, BusinessException>> Try(Action act)
        {
            return await Try(() =>
            {
                act();
                return true;
            });
        }

        public static Result<T, BusinessException> From<T> (T data)
        {
            return new Result<T, BusinessException>(data);
        }
    }
}
