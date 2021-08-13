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
            CreateMap<InvoiceDTO, Invoice>();

            CreateMap<InvoiceItemDTO, InvoiceItem>();

            CreateMap<InvoicePaidEvent, RegisterPaidInvoiceCommand>();
        }
    }
}