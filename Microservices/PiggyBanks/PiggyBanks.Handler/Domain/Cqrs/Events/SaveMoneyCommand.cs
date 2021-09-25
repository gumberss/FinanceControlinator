using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using MediatR;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Handler.Domain.Cqrs.Events
{
    public class SaveMoneyCommand : IRequest<Result<PiggyBank, BusinessException>>
    {
        public decimal Value { get; set; }
    }
}
