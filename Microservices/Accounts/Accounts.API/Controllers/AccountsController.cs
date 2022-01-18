using Accounts.API.Commons;
using Accounts.Handler.Domain.Cqrs.Events.Commands;
using Accounts.Handler.Domain.Cqrs.Events.Queries;
using AutoMapper;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Events.PiggyBanks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ApiControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IMessageBus _bus;

        public AccountsController(
            ILogger<AccountsController> logger,
            IMediator mediator,
            IMapper mapper,
            IMessageBus bus)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _bus = bus;
        }
        [HttpGet]
        public async Task<IActionResult> GetClosedItems()
        {
            return From(await _mediator.Send(new AccountDataQuery()));
        }

        [HttpPut("money/withdraw/{id}")]
        public async Task<IActionResult> WithdrawMoney([FromBody] AccountWithdrawMoneyCommand paymentCommand)
        {
            return From(await _mediator.Send(paymentCommand));
        }

        [HttpPut("money")]
        public async Task<IActionResult> PutMoney([FromBody] AccountReceiveMoneyCommand receiveMoneyCommand)
        {
            return From(await _mediator.Send(receiveMoneyCommand));
        }

        [HttpPut("money/save")]
        public async Task<IActionResult> SaveMoney([FromBody] AccountWithdrawForSaveMoneyCommand receiveMoneyCommand)
        {
            var result = await _mediator.Send(receiveMoneyCommand);

            var @event = _mapper.Map<AccountWithdrawForSaveMoneyCommand, SaveMoneyEvent>(receiveMoneyCommand);

            await _bus.Publish(@event);

            return From(result);

        }
    }
}
