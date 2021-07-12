using Invoices.Domain.Models;
using FluentValidation;

namespace Invoices.Domain.Interfaces.Validators
{
    public interface IInvoiceValidator : IValidator<Invoice>
    {
    }
}
