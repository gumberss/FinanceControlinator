using PiggyBanks.API.Commons;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PiggyBanks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PiggyBanksController : ApiControllerBase
    {
        private readonly ILogger<PiggyBanksController> _logger;
        private readonly IMediator _mediator;

        public PiggyBanksController(ILogger<PiggyBanksController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PiggyBank piggyBank)
        {
            return From(await _mediator.Send(new RegisterPiggyBankCommand { PiggyBank = piggyBank }));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return From(await _mediator.Send(new GetAllPiggyBanksQuery()));
        }

    }
}
