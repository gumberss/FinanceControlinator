using CleanHandling;
using Invoices.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class RegisterExpenseCommand : IRequest<Result<List<Invoice>, BusinessException>>
    {
        public Expense Expense { get; set; }
    }
}
