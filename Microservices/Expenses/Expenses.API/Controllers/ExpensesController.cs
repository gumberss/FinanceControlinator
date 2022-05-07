using Expenses.API.Commons;
using Expenses.Domain.Models.Expenses;
using Expenses.DTO.Expenses;
using Expenses.Handler.Domain.Cqrs.Events.Expenses;
using Expenses.Handler.Domain.Cqrs.ExpenseOverviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Expenses.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ExpensesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ExpensesController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseDTO expense)
            => UserId is null ? Unauthorized()
            : From(await _mediator.Send(new RegisterExpenseCommand { Expense = expense with { UserId = UserId.Value} }));

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Expense expense)
            => From(await _mediator.Send(new UpdateExpenseCommand { Expense = expense }));

        [HttpGet("{page}/{count}")]
        public async Task<IActionResult> Get(int page, int count)
            => From(await _mediator.Send(new GetPaginationExpensesQuery(page, count)));

        [HttpGet("Overview")]
        public async Task<IActionResult> Overview()
            => From(await _mediator.Send(new ExpenseOverviewQuery()));

        [HttpGet("Month")]
        public async Task<IActionResult> GetMonth()
            => From(await _mediator.Send(new GetMonthExpensesQuery()));

        [HttpGet("LastMonth")]
        public async Task<IActionResult> GetLastMonth()
            => From(await _mediator.Send(new GetLastMonthExpensesQuery()));
    }
}
