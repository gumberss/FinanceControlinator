using AutoMapper;
using FinanceControlinator.Events.Invoices.DTOs;
using Invoices.Domain.Models;

namespace Invoices.Handler.Configurations.Profiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<InvoiceExpenseDTO, Expense>();
        }
    }
}