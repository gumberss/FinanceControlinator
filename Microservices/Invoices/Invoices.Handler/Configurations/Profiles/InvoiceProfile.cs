using AutoMapper;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Invoices.DTOs;
using Invoices.Domain.Models;
using System;
using System.Collections.Generic;

namespace Invoices.Handler.Configurations.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)));

            CreateMap<InvoiceItem, InvoiceItemDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)))
                .ForMember(x => x.ExpenseId, x => x.MapFrom(y => Guid.Parse(y.ExpenseId)));

            CreateMap<List<Invoice>, InvoicesChangedEvent>()
                .ForMember(x => x.Invoices, x => x.MapFrom(y => y));
        }
    }
}
