using AutoMapper;
using Expenses.Application.Interfaces.AppServices;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.ExpenseOverviews
{
    public class ExpenseOverviewHandler : IRequestHandler<ExpenseOverviewQuery, Result<ExpenseOverviewDTO, BusinessException>>
    {
        private readonly IExpenseOverviewAppService _expenseOverviewAppService;
        private readonly IMapper _mapper;

        public ExpenseOverviewHandler(IExpenseOverviewAppService expenseOverviewAppService,
            IMapper mapper)
        {
            _expenseOverviewAppService = expenseOverviewAppService;
            _mapper = mapper;
        }

        public async Task<Result<ExpenseOverviewDTO, BusinessException>> Handle(ExpenseOverviewQuery request, CancellationToken cancellationToken)
        {
            var result = await _expenseOverviewAppService.GetExpensesOverview(request.UserId);

            if (result.IsFailure) return result.Error;

            return _mapper.Map<ExpenseOverviewDTO>(result.Value);
        }
    }
}
