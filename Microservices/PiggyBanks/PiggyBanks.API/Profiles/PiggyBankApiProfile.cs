using AutoMapper;
using FinanceControlinator.Events.PiggyBanks;
using FinanceControlinator.Events.PiggyBanks.DTOs;
using PiggyBanks.Domain.Models;

namespace PiggyBanks.API.Profiles
{
    public class PiggyBankApiProfile : Profile
    {
        public PiggyBankApiProfile()
        {
            CreateMap<PiggyBank, PiggyBankCreatedEvent>()
                .ForMember(x => x.PiggyBank, x => x.MapFrom(y => y));

            CreateMap<PiggyBank, PiggyBankDTO>();
        }
    }
}