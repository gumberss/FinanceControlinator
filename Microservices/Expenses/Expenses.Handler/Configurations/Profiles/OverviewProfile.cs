using AutoMapper;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses.Overviews;
using Expenses.Handler.Domain.Cqrs.ExpenseOverviews;
using System;

namespace Expenses.Handler.Configurations.Profiles
{
    public class OverviewProfile : Profile
    {
        public OverviewProfile(IServiceProvider s)
        {
            var localization = (ILocalization)s.GetService((typeof(ILocalization)));

            CreateMap<ExpenseOverview, ExpenseOverviewDTO>();

            CreateMap<ExpenseBrief, ExpenseBriefDTO>();

            CreateMap<ExpensePartition, ExpensePartitionDTO>()
                .ForMember(x => x.Type, x => x.MapFrom(y => localization.EXPENSE_TYPE(y.Type)));
        }
    }
}
