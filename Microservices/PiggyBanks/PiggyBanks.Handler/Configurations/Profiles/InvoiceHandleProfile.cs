using AutoMapper;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Invoices.DTOs;
using PiggyBanks.Domain.Models;
using PiggyBanks.Handler.Domain.Cqrs.Events.Invoices;
using PiggyBanks.Handler.Integration;
using PiggyBanks.Handler.Integration.Events.Invoices;

namespace PiggyBanks.Handler.Configurations.Profiles
{
    class InvoiceHandleProfile : Profile
    {
        public InvoiceHandleProfile()
        {
            CreateMap<InvoicePaidEvent, InvoicePaidCommand>();
            CreateMap<InvoiceDTO, Invoice>();

            CreateMap<InvoiceItemDTO, InvoiceItem>()
                .ForMember(x => x.Invoice, x => x.MapFrom(y => (Invoice)null));

            CreateMap<Invoice, PiggyBankPaidInvoiceRegisteredEvent>()
                .ForMember(x => x.Invoice, x => x.MapFrom(y => y));
        }
    }
}
