using CleanHandling;
using MediatR;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Handler.Domain.Cqrs.Events
{
    public class SaveMoneyCommand : IRequest<Result<PiggyBank, BusinessException>>
    {
        public decimal Amount { get; set; }
    }
}
