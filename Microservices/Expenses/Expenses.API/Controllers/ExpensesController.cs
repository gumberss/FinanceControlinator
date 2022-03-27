﻿using Expenses.API.Commons;
using Expenses.Domain.Models.Expenses;
using Expenses.Handler.Domain.Cqrs.Events.Expenses;
using Expenses.Handler.Domain.Cqrs.ExpenseOverviews;
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

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Expense expense)
        {
            return From(await _mediator.Send(new UpdateExpenseCommand { Expense = expense }));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return From(await _mediator.Send(new GetAllExpensesQuery()));
        }

        [HttpGet("Overview")]
        public async Task<IActionResult> Overview()
        {
            return From(await _mediator.Send(new ExpenseOverviewQuery()));
        }

        [HttpGet("Month")]
        public async Task<IActionResult> GetMonth()
        {
            return From(await _mediator.Send(new GetMonthExpensesQuery()));
        }

        [HttpGet("LastMonth")]
        public async Task<IActionResult> GetLastMonth()
        {
            return From(await _mediator.Send(new GetLastMonthExpensesQuery()));
        }
    }
}
