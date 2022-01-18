using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Handler.Domain.Cqrs.Events.Transfers
{
    public class RegisterTransferCommand : IRequest<Result<Transfer, BusinessException>>
    {
        public Transfer Transfer { get; set; }
    }
}
