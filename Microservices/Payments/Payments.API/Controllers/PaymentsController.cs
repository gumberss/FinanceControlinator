using Payments.API.Commons;
using Payments.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Payments.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ApiControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IMediator _mediator;

        public PaymentsController(ILogger<PaymentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Invoice invoice)
        {
            //return From(await _mediator.Send(new PayInvoiceCommand { Invoice = invoice }));

            return Ok();
        }

        
    }
}
