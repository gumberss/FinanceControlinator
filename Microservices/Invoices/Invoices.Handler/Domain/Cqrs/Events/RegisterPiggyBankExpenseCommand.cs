using CleanHandling;
using Invoices.Domain.DTOs;
using Invoices.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class RegisterPiggyBankExpenseCommand : IRequest<Result<List<Invoice>, BusinessException>>
    {
        public InvoicePiggyBank Expense { get; set; }
    }
}
