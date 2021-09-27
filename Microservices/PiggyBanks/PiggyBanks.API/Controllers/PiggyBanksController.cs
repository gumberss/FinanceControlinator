using PiggyBanks.API.Commons;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MassTransit;
using AutoMapper;
using FinanceControlinator.Events.PiggyBanks;

namespace PiggyBanks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PiggyBanksController : ApiControllerBase
    {
        private readonly ILogger<PiggyBanksController> _logger;
        private readonly IMediator _mediator;
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public PiggyBanksController(
            ILogger<PiggyBanksController> logger, 
            IMediator mediator,
            IBus bus,
            IMapper mapper
            )
        {
            _logger = logger;
            _mediator = mediator;
            _bus = bus;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PiggyBank piggyBank)
        {
            var registerCommand = await _mediator.Send(new RegisterPiggyBankCommand { PiggyBank = piggyBank });

            if (registerCommand.IsFailure)  
                return From(registerCommand);

            var @event = _mapper.Map<PiggyBank, PiggyBankCreatedEvent>(registerCommand.Value);

            await _bus.Publish(@event);

            return From(registerCommand);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return From(await _mediator.Send(new GetAllPiggyBanksQuery()));
        }

    }
}
