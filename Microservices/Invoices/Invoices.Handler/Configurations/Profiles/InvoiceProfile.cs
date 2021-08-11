using AutoMapper;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Invoices.DTOs;
using Invoices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Handler.Configurations.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)));

            CreateMap<InvoiceItem, InvoiceItemDTO>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)));

            CreateMap<List<Invoice>, InvoicesChangedEvent>()
                .ForMember(x => x.Invoices, x => x.MapFrom(y => y));
        }
    }
}
