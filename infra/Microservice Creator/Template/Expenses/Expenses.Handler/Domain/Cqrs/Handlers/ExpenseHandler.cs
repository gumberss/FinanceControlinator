﻿using AutoMapper;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Domain.Models;
using Expenses.Handler.Domain.Cqrs.Events;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Events.Expenses;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Handler.Domain.Cqrs.Handlers
{
    public class ExpenseHandler
        : IRequestHandler<RegisterExpenseCommand, Result<Expense, BusinessException>>
        , IRequestHandler<GetAllExpensesQuery, Result<List<Expense>, BusinessException>>
        , IRequestHandler<GetMonthExpensesQuery, Result<List<Expense>, BusinessException>>
        , IRequestHandler<GetLastMonthExpensesQuery, Result<List<Expense>, BusinessException>>
    {
        private readonly IExpenseAppService _expenseAppService;
        private readonly ILogger<ExpenseHandler> _logger;
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public ExpenseHandler(
            IExpenseAppService expenseAppService
            , ILogger<ExpenseHandler> logger
            , IBus bus,
            IMapper mapper)
        {
            _expenseAppService = expenseAppService;
            _logger = logger;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<Result<Expense, BusinessException>> Handle(RegisterExpenseCommand request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(this.GetType().Name))
            {
                var result = await _expenseAppService.RegisterExpense(request.Expense);

                if (result.IsFailure)
                {
                    return result;
                }

                var @event = _mapper.Map<Expense, ExpenseCreatedEvent>(result.Value);
                
                await _bus.Publish(@event);

                return result;
            }
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
    }
}
