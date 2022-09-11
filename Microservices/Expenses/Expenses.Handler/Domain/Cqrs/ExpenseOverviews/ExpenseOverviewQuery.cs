using CleanHandling;
using MediatR;
using System;

namespace Expenses.Handler.Domain.Cqrs.ExpenseOverviews
{
    public class ExpenseOverviewQuery : IRequest<Result<ExpenseOverviewDTO, BusinessException>>
    {
        public Guid UserId { get; set; }

        public ExpenseOverviewQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
