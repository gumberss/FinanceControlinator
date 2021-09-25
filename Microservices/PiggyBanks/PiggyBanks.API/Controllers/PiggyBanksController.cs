using PiggyBanks.API.Commons;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MassTransit;

namespace PiggyBanks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PiggyBanksController : ApiControllerBase
    {
        private readonly ILogger<PiggyBanksController> _logger;
        private readonly IMediator _mediator;
        private readonly IBus _bus;

        public PiggyBanksController(
            ILogger<PiggyBanksController> logger, 
            IMediator mediator,
            IBus bus
            )
        {
            _logger = logger;
            _mediator = mediator;
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PiggyBank piggyBank)
        {
            var registerCommand = await _mediator.Send(new RegisterPiggyBankCommand { PiggyBank = piggyBank });

            if (registerCommand.IsFailure)  
                return From(registerCommand);

            //_bus.Publish(new PiggyBankCreatedEvent);

            return From(registerCommand);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return From(await _mediator.Send(new GetAllPiggyBanksQuery()));
        }

    }
}
