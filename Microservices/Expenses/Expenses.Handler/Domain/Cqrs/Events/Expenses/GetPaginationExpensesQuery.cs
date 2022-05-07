using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System;
using System.Collections.Generic;

namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class GetPaginationExpensesQuery : IRequest<Result<List<Expense>, BusinessException>>
    {
        public int Page { get; set; }

        public int Count { get; set; }

        public Guid UserId { get; set; }

        public GetPaginationExpensesQuery(int page, int count, Guid userId)
        {
            Page = page;
            Count = count;
            UserId = userId;
        }
    }
}
