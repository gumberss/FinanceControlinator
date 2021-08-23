using Invoices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Domain.Services
{
    public interface IInvoiceService
    {
        DateTime GetCurrentInvoiceCloseDate();

        (DateTime startDate, DateTime endDate) GetInvoiceRangeByInstallments(int installmentsCount);

        List<Invoice> RegisterExpense(
            Expense expense
          , List<Invoice> existentInvoices
          , DateTime currentInvoiceDate
        );
    }

    public class InvoiceService : IInvoiceService
    {
        public DateTime GetCurrentInvoiceCloseDate()
        {
            var now = DateTime.Now;

            var closeDay = DateTime.DaysInMonth(now.Year, now.Month);/*Todo: config - Close date*/

            var currentInvoiceDate = new DateTime(now.Year, now.Month, closeDay);

            if (currentInvoiceDate >= now)
            {
                currentInvoiceDate.AddMonths(1);
            }

            return currentInvoiceDate;
        }

        public (DateTime startDate, DateTime endDate) GetInvoiceRangeByInstallments(int installmentsCount)
        {
            var currentInvoiceCloseDate = GetCurrentInvoiceCloseDate();

            var endDate = currentInvoiceCloseDate.AddMonths(installmentsCount);

            var startDate = new DateTime(currentInvoiceCloseDate.Year, currentInvoiceCloseDate.Month, DateTime.Now.Day);

            return (startDate, endDate);
        }

        public List<Invoice> RegisterExpense(
            Expense expense
          , List<Invoice> existentInvoices
          , DateTime currentInvoiceDate
        )
        {
            var installmentsCount = expense.InstallmentsCount;

            var changedInvoices = new List<Invoice>(installmentsCount);

            for (int i = 0; i < installmentsCount; i++)
            {
                var currentInstallment = i + 1;

                var currentMonth = currentInvoiceDate.AddMonths(i);

                var currentInstallmentInvoice = existentInvoices.Find(
                  x => x.CloseDate.Month == currentMonth.Month
                    && x.CloseDate.Year == currentMonth.Year
                );

                var invoiceItem = GetInvoceItemFrom(expense, currentInstallment);

                if (currentInstallmentInvoice is null)
                {
                    changedInvoices.Add(new Invoice(currentMonth).AddNew(invoiceItem));
                }
                else
                {
                    changedInvoices.Add(currentInstallmentInvoice
                        .AddNew(invoiceItem)
                        .WasUpdated());
                }
            }

            return changedInvoices;
        }

        private InvoiceItem GetInvoceItemFrom(Expense expense, int installment)
        {
            var totalExpenseCost = expense.TotalCost;

            var expenseInstallmentsCount = expense.InstallmentsCount;

            var normalInstallmentCost = Math.Round(totalExpenseCost / expenseInstallmentsCount, 2);

            decimal installmentCost;

            if (installment < expenseInstallmentsCount)
            {
                installmentCost = normalInstallmentCost;
            }
            else
            {
                var totalOtherInstallmentCosts = normalInstallmentCost * (expenseInstallmentsCount - 1);

                var restOfCost = totalExpenseCost - totalOtherInstallmentCosts;

                installmentCost = restOfCost;
            }

            return new InvoiceItem(installment, installmentCost)
                .From(expense);
        }
    }
}
