using AutoMapper;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Events.Invoices;
using FinanceControlinator.Events.Invoices.DTOs;

namespace Expenses.Handler.Configurations.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Expense, GenerateInvoicesEvent>()
                .ForMember(x => x.InvoiceExpense, x => x.MapFrom(y => y));

            CreateMap<Expense, InvoiceExpenseDTO>()
                .ForMember(x => x.DetailsPath, x => x.MapFrom(y => $"expenses/{y.Id}"));
        }
    }
}
