using AutoMapper;
using FinanceControlinator.Events.Payments;
using FinanceControlinator.Events.Payments.DTOs;
using Invoices.Domain.Models;
using Invoices.Handler.Domain.Cqrs.Events;

namespace Invoices.Handler.Configurations.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentPerformedEvent, RegisterInvoicePaymentCommand>();

            CreateMap<PaymentDTO, Payment>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id.ToString()))
                .ForMember(x => x.ItemId, x => x.MapFrom(y => y.ItemId.ToString()));
        }
    }
}
