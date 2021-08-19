using Accounts.API.Commons;
using Accounts.Domain.Models;
using Accounts.Handler.Domain.Cqrs.Events.Commands;
using Accounts.Handler.Domain.Cqrs.Events.Queries;
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

        public AccountsController(ILogger<AccountsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
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

        [HttpPut("money/{id}")]
        public async Task<IActionResult> PutMoney([FromBody] AccountReceiveMoneyCommand paymentCommand)
        {
            return From(await _mediator.Send(new AccountReceiveMoneyCommand()));
        }
    }
}
