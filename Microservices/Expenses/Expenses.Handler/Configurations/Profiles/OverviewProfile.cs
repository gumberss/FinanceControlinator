using AutoMapper;
using Expenses.Domain.Models.Expenses.Overviews;
using Expenses.Handler.Domain.Cqrs.ExpenseOverviews;

namespace Expenses.Handler.Configurations.Profiles
{
    public class OverviewProfile : Profile
    {
        public OverviewProfile()
        {
            CreateMap<ExpenseOverview, ExpenseOverviewDTO>();

            CreateMap<ExpenseBrief, ExpenseBriefDTO>();

            CreateMap<ExpensePartition, ExpensePartitionDTO>();
        }
    }
}
