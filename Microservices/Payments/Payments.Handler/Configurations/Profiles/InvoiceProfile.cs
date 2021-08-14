using AutoMapper;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Invoices.DTOs;
using Payments.Domain.Models;
using Payments.Handler.Domain.Cqrs.Events.Commands;

namespace Payments.Handler.Configurations.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<InvoicesChangedEvent, AddOrUpdateInvoicesCommand>();

            CreateMap<InvoiceItemDTO, InvoiceItem>();
            CreateMap<InvoiceDTO, Invoice>();

        }
    }
}
