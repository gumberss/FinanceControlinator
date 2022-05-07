using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Expenses.API.Commons
{
    public class ApiControllerBase : ControllerBase
    {
        protected Guid? UserId => 
                Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out Guid id)
                    ? id
                    : null;

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