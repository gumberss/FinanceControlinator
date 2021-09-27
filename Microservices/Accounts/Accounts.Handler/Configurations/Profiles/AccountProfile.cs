using AutoMapper;
using Accounts.Domain.Models;
using FinanceControlinator.Events.Payments.DTOs;
using FinanceControlinator.Events.Payments;
using Accounts.Handler.Domain.Cqrs.Events.Commands.Payments;
using System;
using FinanceControlinator.Events.Accounts;
using System.Collections.Generic;
using FinanceControlinator.Events.Accounts.DTOs;
using Accounts.Handler.Domain.Cqrs.Events.Commands;
using FinanceControlinator.Events.PiggyBanks;

namespace Accounts.Handler.Configurations.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<PaymentRequestedEvent, PayCommand>();

            CreateMap<PaymentDTO, Payment>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id.ToString()))
                .ForMember(x => x.ItemId, x => x.MapFrom(y => y.ItemId.ToString()));

            CreateMap<PaymentMethodDTO, PaymentMethod>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id.ToString()))
                .ForMember(x => x.AmountSourceId, x => x.MapFrom(y => y.AmountSourceId.ToString()));

            CreateMap<AccountChange, PaymentConfirmedEvent>()
                .ForMember(x=> x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)));

            CreateMap<List<AccountChange>, AccountChangedEvent>()
                .ForMember(x => x.AccountChangeDTOs, x => x.MapFrom(y => y));

            CreateMap<AccountChange, AccountChangeDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)))
                .ForMember(x => x.PaymentId, x => x.MapFrom(y => Guid.Parse(y.PaymentId)));


            CreateMap<AccountWithdrawForSaveMoneyCommand, SaveMoneyEvent>();
        }
    }
}
