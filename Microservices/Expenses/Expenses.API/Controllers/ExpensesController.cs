using Expenses.API.Commons;
using Expenses.Application.Domain.Cqrs.Events;
using Expenses.Data.Contexts;
using Expenses.Data.Repositories;
using Expenses.Domain.Models;
using FinanceControlinator.Common;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Expenses.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpensesController : ApiControllerBase
    {
        private readonly ILogger<ExpensesController> _logger;
        private readonly IMediator _mediator;

        public ExpensesController(ILogger<ExpensesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Expense expense)
        {
            return From(await _mediator.Send(new RegisterExpenseCommand { Expense = expense }));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] Expense expense)
        {
            return Ok("Ok");
        }
    }
}
