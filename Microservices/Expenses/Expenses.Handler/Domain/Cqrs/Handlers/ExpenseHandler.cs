using Expenses.Application.Domain.Cqrs.Events;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.Handlers
{
    public class ExpenseHandler
        : IRequestHandler<RegisterExpenseCommand, Result<Expense, BusinessException>>
    {
        private readonly IExpenseAppService _expenseAppService;

        public ExpenseHandler(IExpenseAppService  expenseAppService)
        {
            _expenseAppService = expenseAppService;
        }

        public Task<Result<Expense, BusinessException>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
        {
            _expenseAppService.RegisterExpense(request.Expense);


            return Task.FromResult(new Result<Expense, BusinessException>(request.Expense));
        }

    }
}
