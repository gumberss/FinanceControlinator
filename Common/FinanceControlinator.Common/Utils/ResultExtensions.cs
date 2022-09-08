using FinanceControlinator.Common.Contexts;
using FinanceControlinator.Common.Exceptions;
using System;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.Utils
{
    public static class ResultExtensions
    {
        public async static Task<Result<Tr, BusinessException>> Then<Ti, Tr>
           (this Task<Result<Ti, BusinessException>> input,
           Func<Ti, Task<Result<Tr, BusinessException>>> func) where Tr : class
        {
            var result = await input;

            return result.IsFailure ?
                result.Error
                : await func(result.Value);
        }

        public async static Task<Result<Tr, BusinessException>> Then<Ti, Tr>
             (this Task<Result<Ti, BusinessException>> input,
             Func<Ti, Result<Tr, BusinessException>> func) where Tr : class
        {
            var result = await input;

            return result.IsFailure
                ? result.Error
                : func(result.Value);
        }

        public async static Task<Result<Tr, BusinessException>> Then<Ti, Tr>
            (this Result<Ti, BusinessException> input,
            Func<Ti, Task<Result<Tr, BusinessException>>> func) where Tr : class
        {
            var result = input;

            return result.IsFailure
                 ? result.Error
                 : await func(result.Value);
        }

        public async static Task<Result<Tr, BusinessException>> Then<Ti, Tr>
            (this Task<Result<Ti, BusinessException>> input,
            Func<Ti, Tr> func)
        {
            var result = await input;

            return result.IsFailure
                ? result.Error
                : func(result.Value);
        }

        public async static Task<Result<Tr, BusinessException>> When<Ti, Tr>
            (this Task<Result<Ti, BusinessException>> input,
            Func<Ti, bool> condiction,
            Func<Ti, Task<Result<Tr, BusinessException>>> @then,
            Func<Ti, Task<Result<Tr, BusinessException>>> @else = null) where Tr : class
        {
            var result = await input;

            if (result.IsFailure) return result.Error;

            return condiction(result.Value)
                ? await @then(result.Value)
                : @else is not null
                    ? await @else(result.Value)
                    : Result.From(default(Tr));
        }

        public async static Task<Result<Tr, BusinessException>> When<Ti, Tr>
             (this Task<Result<Ti, BusinessException>> input,
             Func<Ti, bool> condiction,
             Func<Ti, Result<Tr, BusinessException>> @then,
             Func<Ti, Result<Tr, BusinessException>> @else = null) where Tr : class
        {
            var result = await input;

            if (result.IsFailure) return result.Error;

            return condiction(result.Value)
                ? @then(result.Value)
                : @else is not null
                    ? @else(result.Value)
                    : Result.From(default(Tr));
        }

        public async static Task<Result<Tr, BusinessException>> When<Ti, Tr>
            (this Result<Ti, BusinessException> input,
            Func<Ti, bool> condiction,
            Func<Ti, Task<Result<Tr, BusinessException>>> @then,
            Func<Ti, Task<Result<Tr, BusinessException>>> @else = null) where Tr : class
        {
            if (input.IsFailure) return input.Error;

            return condiction(input.Value)
                ? await @then(input.Value)
                 : @else is not null
                     ? await @else(input.Value)
                     : Result.From(default(Tr));
        }

        public async static Task<Result<Tr, BusinessException>> When<Ti, Tr>
         (this Task<Result<Ti, BusinessException>> input,
            Func<Ti, bool> condiction,
            Func<Ti, Tr> @then,
            Func<Ti, Tr> @else = null)
        {
            var result = await input;

            if (result.IsFailure)
                return result.Error;

            return condiction(result.Value)
                ? @then(result.Value)
               : @else is not null
                   ? @else(result.Value)
                   : Result.From(default(Tr));
        }
    }
}
