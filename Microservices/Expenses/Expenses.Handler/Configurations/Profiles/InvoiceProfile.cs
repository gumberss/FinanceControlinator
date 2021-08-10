﻿using AutoMapper;
using Expenses.Domain.Models.Invoices;
using Expenses.Handler.Domain.Cqrs.Events.Invoices;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Invoices.DTOs;
using System;

namespace Expenses.Handler.Configurations.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<InvoiceDTO, Invoice>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)));

            CreateMap<InvoiceItemDTO, InvoiceItem>()
                .ForMember(x => x.Id, x => x.MapFrom(y => Guid.Parse(y.Id)))
                .ForMember(x => x.ExpenseId, x => x.MapFrom(y => Guid.Parse(y.ExpenseId)));

            CreateMap<InvoicesChangedEvent, ChangeInvoicesCommand>();
        }
    }
}
