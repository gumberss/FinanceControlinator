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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Invoice invoice)
        {
            return From(await _mediator.Send(new RegisterInvoiceCommand { Invoice = invoice }));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return From(await _mediator.Send(new GetAllInvoicesQuery ()));
        }

        [HttpGet("Month")]
        public async Task<IActionResult> GetMonth()
        {
            return From(await _mediator.Send(new GetMonthInvoicesQuery()));
        }

        [HttpGet("LastMonth")]
        public async Task<IActionResult> GetLastMonth()
        {
            return From(await _mediator.Send(new GetLastMonthInvoicesQuery()));
        }
    }
}
