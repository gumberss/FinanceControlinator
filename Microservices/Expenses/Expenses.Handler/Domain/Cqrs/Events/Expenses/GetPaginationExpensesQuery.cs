﻿using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using System.Collections.Generic;

namespace Expenses.Handler.Domain.Cqrs.Events.Expenses
{
    public class GetPaginationExpensesQuery : IRequest<Result<List<Expense>, BusinessException>>
    {
        public int Page { get; set; }

        public int Count { get; set; }

        public GetPaginationExpensesQuery(int page, int count)
        {
            Page = page;
            Count = count;
        }
    }
}
