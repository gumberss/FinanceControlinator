using Invoices.API.Commons;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Invoices.API.Controllers
{
    [ApiController]
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

    }
}
