using PiggyBanks.Domain.Models;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;

namespace PiggyBanks.Handler.Domain.Cqrs.Events
{
    public class RegisterPiggyBankCommand : IRequest<Result<PiggyBank, BusinessException>>
    {
        public PiggyBank PiggyBank { get; set; }
    }
}
