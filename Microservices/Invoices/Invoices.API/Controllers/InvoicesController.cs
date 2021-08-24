using Invoices.API.Commons;
using Invoices.Domain.Models;
using Invoices.Handler.Domain.Cqrs.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
