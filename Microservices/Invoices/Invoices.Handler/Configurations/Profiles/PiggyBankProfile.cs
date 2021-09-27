using AutoMapper;
using FinanceControlinator.Events.PiggyBanks;
using FinanceControlinator.Events.PiggyBanks.DTOs;
using Invoices.Domain.DTOs;
using Invoices.Domain.Enums;
using Invoices.Handler.Domain.Cqrs.Events;

namespace Invoices.Handler.Configurations.Profiles
{
    public class PiggyBankProfile : Profile
    {
        public PiggyBankProfile()
        {
            CreateMap<PiggyBankCreatedEvent, RegisterPiggyBankExpenseCommand>()
                .ForMember(x => x.Expense, x => x.MapFrom(y => y.PiggyBank));

            CreateMap<PiggyBankDTO, InvoicePiggyBank>()
                .ForMember(x => x.Type, x => x.MapFrom(y => (InvoiceItemType)y.Type));

        }
    }
}
