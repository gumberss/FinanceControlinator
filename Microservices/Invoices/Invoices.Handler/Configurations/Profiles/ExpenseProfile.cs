using AutoMapper;
using FinanceControlinator.Events.Expenses;
using FinanceControlinator.Events.Expenses.DTOs;
using Invoices.Domain.Models;

namespace Invoices.Handler.Configurations.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense, ExpenseCreatedEvent>()
                .ForMember(x => x.Expense, x => x.MapFrom(y => y))
                .ReverseMap();

            CreateMap<Expense, ExpenseDTO>()
                .ReverseMap();
            CreateMap<ExpenseItem, ExpenseItemDTO>()
                .ReverseMap();
        }
    }
}