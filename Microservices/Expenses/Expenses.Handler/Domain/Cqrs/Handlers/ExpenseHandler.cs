using Expenses.Application.Domain.Cqrs.Events;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
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

        public ExpenseHandler(
            IExpenseAppService expenseAppService
            , ILogger<ExpenseHandler> logger)
        {
            _expenseAppService = expenseAppService;
            _logger = logger;
        }

        public Task<Result<Expense, BusinessException>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(this.GetType().Name))
            {
                return _expenseAppService.RegisterExpense(request.Expense);
            }
        }

    }
}
