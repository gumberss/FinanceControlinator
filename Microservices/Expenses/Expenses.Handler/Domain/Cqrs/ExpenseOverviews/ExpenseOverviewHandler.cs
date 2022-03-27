using AutoMapper;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.ExpenseOverviews
{
    public class ExpenseOverviewHandler : IRequestHandler<ExpenseOverviewQuery, Result<List<ExpenseOverviewDTO>, BusinessException>>
    {
        private readonly IExpenseOverviewAppService _expenseOverviewAppService;
        private readonly IMapper _mapper;

        public ExpenseOverviewHandler(IExpenseOverviewAppService expenseOverviewAppService,
            IMapper mapper)
        {
            _expenseOverviewAppService = expenseOverviewAppService;
            _mapper = mapper;
        }

        public async Task<Result<List<ExpenseOverviewDTO>, BusinessException>> Handle(ExpenseOverviewQuery request, CancellationToken cancellationToken)
        {
            var result = await _expenseOverviewAppService.GetExpensesOverview();

            if (result.IsFailure) return result.Error;

            return _mapper.Map<List<ExpenseOverviewDTO>>(result.Value);
        }
    }
}
