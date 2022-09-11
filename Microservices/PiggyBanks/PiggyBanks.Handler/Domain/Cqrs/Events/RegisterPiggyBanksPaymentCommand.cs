using CleanHandling;
using MediatR;
using PiggyBanks.Domain.Models;
using System.Collections.Generic;

namespace PiggyBanks.Handler.Domain.Cqrs.Events
{
    public class RegisterPiggyBanksPaymentCommand : IRequest<Result<List<PiggyBank>, BusinessException>>
    {
        public Invoice Invoice { get; set; }
    }
}
