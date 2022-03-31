using AutoMapper;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Interfaces.Contexts;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses;
using Expenses.Handler.Domain.Cqrs.Events.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Messaging;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Events.Invoices;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.Handlers
{
    public class ExpenseHandler
        : IRequestHandler<RegisterExpenseCommand, Result<Expense, BusinessException>>
        , IRequestHandler<UpdateExpenseCommand, Result<Expense, BusinessException>>
        , IRequestHandler<GetAllExpensesQuery, Result<List<Expense>, BusinessException>>
        , IRequestHandler<GetMonthExpensesQuery, Result<List<Expense>, BusinessException>>
        , IRequestHandler<GetLastMonthExpensesQuery, Result<List<Expense>, BusinessException>>
        , IRequestHandler<GetPaginationExpensesQuery, Result<List<Expense>, BusinessException>>
    {
        private readonly IExpenseAppService _expenseAppService;
        private readonly ILogger<ExpenseHandler> _logger;
        private readonly IMessageBus _bus;
        private readonly IMapper _mapper;
        private readonly IExpenseDbContext _expenseDbContext;
        private readonly ILocalization _localization;

        public ExpenseHandler(
            IExpenseAppService expenseAppService
            , IExpenseDbContext expenseDbContext
            , ILocalization localization
            , ILogger<ExpenseHandler> logger
            , IMessageBus bus,
            IMapper mapper)
        {
            _expenseAppService = expenseAppService;
            _localization = localization;
            _logger = logger;
            _bus = bus;
            _mapper = mapper;
            _expenseDbContext = expenseDbContext;
        }

        public async Task<Result<Expense, BusinessException>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(this.GetType().Name))
            {
                var result = await _expenseAppService.RegisterExpense(request.Expense);

                if (result.IsFailure) return result;

                var saveResult = await Result.Try(_expenseDbContext.Commit());

                if (saveResult.IsFailure)
                {
                    var errorData = new ErrorData(_localization.AN_ERROR_OCCURRED_ON_THE_SERVER);
                    var exception = new BusinessException(HttpStatusCode.InternalServerError, errorData);

                    _logger.LogError(saveResult.Error, exception.Log());

                    return exception;
                }

                var generateInvoicesEvent = _mapper.Map<Expense, GenerateInvoicesEvent>(result.Value);

                await _bus.Publish(generateInvoicesEvent);

                return result;
            }
        }

        public async Task<Result<Expense, BusinessException>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var result = await _expenseAppService.UpdateExpense(request.Expense);

            if (result.IsFailure) return result;

            var saveResult = await Result.Try(_expenseDbContext.Commit());

            if (saveResult.IsFailure)
            {
                var errorData = new ErrorData(_localization.AN_ERROR_OCCURRED_ON_THE_SERVER);
                var exception = new BusinessException(HttpStatusCode.InternalServerError, errorData);

                _logger.LogError(saveResult.Error, exception.Log());

                return exception;
            }

            //var generateInvoicesEvent = _mapper.Map<Expense, GenerateInvoicesEvent>(result.Value);

            //await _bus.Publish(generateInvoicesEvent);

            return result;
        }

        public async Task<Result<List<Expense>, BusinessException>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            return await _expenseAppService.GetAllExpenses();
        }

        public async Task<Result<List<Expense>, BusinessException>> Handle(GetMonthExpensesQuery request, CancellationToken cancellationToken)
        {
            return await _expenseAppService.GetMonthExpenses();
        }

        public async Task<Result<List<Expense>, BusinessException>> Handle(GetLastMonthExpensesQuery request, CancellationToken cancellationToken)
        {
            return await _expenseAppService.GetLastMonthExpenses();
        }

        public async Task<Result<List<Expense>, BusinessException>> Handle(GetPaginationExpensesQuery request, CancellationToken cancellationToken)
        {
            return await _expenseAppService.GetByPagination(request.Page, request.Count);
        }
    }
}
