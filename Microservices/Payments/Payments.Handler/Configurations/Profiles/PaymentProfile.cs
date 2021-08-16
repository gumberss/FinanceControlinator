using AutoMapper;
using FinanceControlinator.Events.Payments;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;

namespace Payments.Handler.Configurations.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<RegisterItemToPayEvent, RegisterPaymentItemCommand>()
                .ForMember(x => x.PaymentItem, x => x.MapFrom(y => y));

            CreateMap<RegisterItemToPayEvent, PaymentItem>();
        }
    }
}
