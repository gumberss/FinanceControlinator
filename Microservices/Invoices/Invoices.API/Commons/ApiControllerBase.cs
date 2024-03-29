using CleanHandling;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Invoices.API.Commons
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

        protected async Task<IActionResult> FromAsync<T, E>(Task<Result<T, E>> task) where E : class
        {
            try
            {
                var result = await task;
                return From(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}