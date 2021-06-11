using Expenses.Application.Domain.Cqrs.Events;
using Expenses.Domain.Models;
using FinanceControlinator.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Expenses.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : ApiControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly IMediator _mediator;

        public ExpenseController(ILogger<ExpenseController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Expense expense)
        {
            return From(await _mediator.Send(new RegisterExpenseCommand { Expense = expense }));
        }

       
    }
}
