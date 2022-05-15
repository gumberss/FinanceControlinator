using AutoMapper;
using Invoices.Domain.Models.Sync;
using Invoices.DTOs.Invoices.Sync;

namespace Invoices.Handler.Configurations.Profiles
{
    internal class InvoiceSyncProfile : Profile
    {
        public InvoiceSyncProfile()
        {
            CreateMap<InvoiceSync, InvoiceSyncDTO>();
            CreateMap<InvoiceMonthDataSync, InvoiceMonthDataDTO>();
            CreateMap<InvoiceOverviewSync, InvoiceOverviewDTO>();
            CreateMap<InvoiceBrief, InvoiceBriefDTO>();
            CreateMap<InvoicePartition, InvoicePartitionDTO>();

        }
    }
}
