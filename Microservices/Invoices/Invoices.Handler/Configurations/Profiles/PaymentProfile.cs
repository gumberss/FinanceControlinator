using AutoMapper;
using FinanceControlinator.Events.Payments;
using Invoices.Domain.Models;

namespace Invoices.Handler.Configurations.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Invoice, RegisterItemToPayEvent>()
                .ForMember(x => x.DetailsPath, x => x.MapFrom(y => $"invoices/{y.Id}"));
        }
    }
}
