using AutoMapper;
using Accounts.Domain.Models;

namespace Accounts.Handler.Configurations.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            //CreateMap<RegisterItemToPayEvent, RegisterAccountItemCommand>()
            //    .ForMember(x => x.AccountItem, x => x.MapFrom(y => y));

            //CreateMap<RegisterItemToPayEvent, AccountItem>();

            //CreateMap<AccountMethodDTO, AccountMethod>()
            //    .ForMember(x => x.AmountSourceId, x => x.MapFrom(y => y.AmountSourceId.ToString()));
        }
    }
}
