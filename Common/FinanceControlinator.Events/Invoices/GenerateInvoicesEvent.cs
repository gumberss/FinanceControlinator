using FinanceControlinator.Events.Invoices.DTOs;

namespace FinanceControlinator.Events.Invoices
{
    public class GenerateInvoicesEvent
    {
        public InvoiceExpenseDTO InvoiceExpense { get; set; }
    }
}
