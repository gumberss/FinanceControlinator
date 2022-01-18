using AutoMapper;
using FinanceControlinator.Events.PiggyBanks;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PiggyBanks.API.Commons;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;
using PiggyBanks.Handler.Domain.Cqrs.Events.Transfers;
using System.Threading.Tasks;

namespace PiggyBanks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ApiControllerBase
    {
        private readonly ILogger<PiggyBanksController> _logger;
        private readonly IMediator _mediator;

        public TransferController(
            ILogger<PiggyBanksController> logger,
            IMediator mediator,
            IBus bus,
            IMapper mapper
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transfer transfer)
        {
            var registerCommand = await _mediator.Send(new RegisterTransferCommand { Transfer = transfer });

            return From(registerCommand);
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    return From(await _mediator.Send());
        //}

    }
}
