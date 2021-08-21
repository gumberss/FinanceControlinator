using AutoMapper;
using FinanceControlinator.Events.Payments;
using FinanceControlinator.Events.Payments.DTOs;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;
using System;

namespace Payments.Handler.Configurations.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<RegisterItemToPayEvent, RegisterPaymentItemCommand>()
                .ForMember(x => x.PaymentItem, x => x.MapFrom(y => y));

            CreateMap<RegisterItemToPayEvent, PaymentItem>();

            CreateMap<PaymentMethodDTO, PaymentMethod>()
                .ForMember(x => x.AmountSourceId, x => x.MapFrom(y => y.AmountSourceId.ToString()));

            CreateMap<Payment, PaymentRequestedEvent>()
                .ForMember(x => x.Payment, x => x.MapFrom(y => y));

            CreateMap<Payment, PaymentDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)))
                .ForMember(x => x.ItemId, x => x.MapFrom(y => Guid.Parse(y.ItemId)));

            CreateMap<PaymentMethod, PaymentMethodDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)))
                .ForMember(x => x.AmountSourceId, x => x.MapFrom(y => Guid.Parse(y.AmountSourceId)));

            CreateMap<PaymentMethodCommandDTO, PaymentMethod>()
                .ForMember(x => x.AmountSourceId, x => x.MapFrom(y => y.AmountSourceId.ToString()));


        }
    }
}
