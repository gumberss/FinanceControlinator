using CleanHandling;
using Microsoft.AspNetCore.Mvc;

namespace Payments.API.Commons
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult From<T, E>(Result<T, E> result) where E : class
        {
            if (result.IsFailure)
            {
                if (result.Error is BusinessException error)
                {
                    return StatusCode(error.Code.GetHashCode(), error.Serialize());
                }
            }

            return Ok(result.Value);
        }
    }
}