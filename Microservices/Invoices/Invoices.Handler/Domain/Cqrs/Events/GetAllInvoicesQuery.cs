using CleanHandling;
using Invoices.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace Invoices.Handler.Domain.Cqrs.Events
{
    public class GetAllInvoicesQuery : IRequest<Result<List<Invoice>, BusinessException>>
    {
    }
}
