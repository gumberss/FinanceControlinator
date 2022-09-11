using CleanHandling;
using MediatR;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.Handler.Domain.Cqrs.Events
{
    public class RegisterPiggyBankCommand : IRequest<Result<PiggyBank, BusinessException>>
    {
        public PiggyBank PiggyBank { get; set; }
    }
}
