using AutoMapper;
using FinanceControlinator.Events.PiggyBanks;
using PiggyBanks.Handler.Domain.Cqrs.Events;

namespace PiggyBanks.Handler.Configurations.Profiles
{
    public class PiggyBankHandlerProfile : Profile
    {
        public PiggyBankHandlerProfile()
        {
            CreateMap<SaveMoneyEvent, SaveMoneyCommand>();
        }
    }
}
