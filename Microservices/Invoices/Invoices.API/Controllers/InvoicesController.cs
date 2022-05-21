using Invoices.API.Commons;
using Invoices.Handler.Domain.Cqrs.Events.Sync;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Invoices.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class InvoicesController : ApiControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IMediator _mediator;

        public InvoicesController(ILogger<InvoicesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("sync")]
        public async Task<IActionResult> Sync([FromQuery] long timestamp)
            => //!UserId.HasValue ? Unauthorized(): 
            From(await _mediator.Send(new InvoiceSyncQuery(timestamp)));
    }
}
