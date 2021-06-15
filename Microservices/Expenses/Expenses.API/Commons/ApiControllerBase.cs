using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Commons
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult From<T, E>(Result<T, E> result) where E : class
        {
            if (result.IsFailure)
            {
                if (result.Error is BusinessException error)
                {
                    if (error is NotFoundException)
                        return NotFound();

                    return BadRequest(error.Serialize());
                }
            }

            return Ok(result.Value);
        }
    }
}