using Expenses.Application.Domain.Cqrs.Events;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Events.Expenses;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.Handlers
{
    public class ExpenseHandler
        : IRequestHandler<RegisterExpenseCommand, Result<Expense, BusinessException>>
    {
        private readonly IExpenseAppService _expenseAppService;
        private readonly ILogger<ExpenseHandler> _logger;
        private readonly IBus _bus;

        public ExpenseHandler(
            IExpenseAppService expenseAppService
            , ILogger<ExpenseHandler> logger
            , IBus bus)
        {
            _expenseAppService = expenseAppService;
            _logger = logger;
            _bus = bus;
        }

        public async Task<Result<Expense, BusinessException>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(this.GetType().Name))
            {
                var expense = await _expenseAppService.RegisterExpense(request.Expense);

                //automapper??
                await _bus.Publish(new ExpenseCreatedEvent
                {
                    //populate
                });

                return expense;
            }
        }
    }
}
