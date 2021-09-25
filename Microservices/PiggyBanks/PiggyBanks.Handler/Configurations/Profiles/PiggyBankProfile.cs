using AutoMapper;
using FinanceControlinator.Events.PiggyBanks;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events;

namespace PiggyBanks.Handler.Configurations.Profiles
{
    public class PiggyBankProfile : Profile
    {
        public PiggyBankProfile()
        {
            CreateMap<SaveMoneyEvent, SaveMoneyCommand>();

        }
    }
}
