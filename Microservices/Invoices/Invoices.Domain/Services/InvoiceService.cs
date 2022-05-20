using Invoices.Domain.Enums;
using Invoices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Domain.Services
{
    public interface IInvoiceService
    {
        DateTime GetInvoiceCloseDateBy(DateTime baseDate);

        (DateTime startDate, DateTime endDate) GetInvoiceDateRangeByInstallments(int installmentsCount, DateTime baseDate);

        List<Invoice> RegisterExpense(
            Expense expense
          , List<Invoice> existentInvoices
          , DateTime currentInvoiceDate);

        int GetInvoiceInstallmentsByDateRange(DateTime startDate, DateTime endDate);

        List<Invoice> LastInvoicesFrom(Invoice invoice, List<Invoice> pastInvoices, int count);

        Func<Invoice, bool> AnyItemChangedSince(DateTime baseDate);

        Func<Invoice, bool> ClosedInvoiceAfter(DateTime invoiceDateToCompare);
        InvoiceStatus Status(Invoice invoice, DateTime baseDate);
        int DaysRemainingToNextStage(Invoice invoice, DateTime baseDate);
        int DaysToOpen(Invoice invoice, DateTime baseDate);
        int OverdueDays(Invoice invoice, DateTime baseDate);
        int DaysToOverdue(Invoice invoice, DateTime baseDate);
        int DaysToClose(Invoice invoice, DateTime baseDate);
        bool IsPaid(Invoice invoice, DateTime baseDate);
        bool IsClosed(Invoice invoice, DateTime baseDate);
        Func<Invoice, bool> AnyChangeSince(DateTime baseDate);
    }

    public class InvoiceService : IInvoiceService
    {
        public DateTime GetInvoiceCloseDateBy(DateTime baseDate)
        {
            var closeDay = DateTime.DaysInMonth(baseDate.Year, baseDate.Month);/*Todo: config - Close date*/

            var invoiceDate = new DateTime(baseDate.Year, baseDate.Month, closeDay);

            return invoiceDate;
        }

        public int GetInvoiceInstallmentsByDateRange(DateTime startDate, DateTime endDate)
            => GetInvoiceInstallmentsByDateRange(startDate, endDate, 0);

        private int GetInvoiceInstallmentsByDateRange(DateTime startDate, DateTime endDate, int monthCount)
        {
            if ((endDate - startDate).Days < 0) return 0;

            var firstInvoiceCloseDate = startDate.AddMonths(monthCount);

            var lastInvoiceCloseDate = GetInvoiceCloseDateBy(endDate);

            if (lastInvoiceCloseDate.Year > firstInvoiceCloseDate.Year
                || lastInvoiceCloseDate.Month > firstInvoiceCloseDate.Month)
            {
                return GetInvoiceInstallmentsByDateRange(startDate, endDate, monthCount + 1);
            }

            return
                endDate.Day >= lastInvoiceCloseDate.Day
             && lastInvoiceCloseDate.Day > firstInvoiceCloseDate.Day
                ? monthCount + 1
                : monthCount;
        }

        public (DateTime startDate, DateTime endDate) GetInvoiceDateRangeByInstallments(int installmentsCount, DateTime baseDate)
        {
            var invoiceCloseDate = GetInvoiceCloseDateBy(baseDate);

            var endDate = GetInvoiceCloseDateBy(invoiceCloseDate.AddMonths(installmentsCount));

            var startDate = new DateTime(invoiceCloseDate.Year, invoiceCloseDate.Month, baseDate.Day);

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

        public List<Invoice> LastInvoicesFrom(Invoice invoice, List<Invoice> pastInvoices, int count)
            => pastInvoices
                .Where(x => x.CloseDate < invoice.CloseDate)
                .OrderByDescending(x => x.CloseDate)
                .Take(count).ToList();

        public Func<Invoice, bool> AnyItemChangedSince(DateTime baseDate)
            => x => x.Items.Any(y => y.CreatedDate > baseDate
                                  || y.UpdatedDate > baseDate);

        public Func<Invoice, bool> AnyChangeSince(DateTime baseDate)
            => invoice => invoice.UpdatedDate > baseDate
                       || invoice.CreatedDate > baseDate
                       || AnyItemChangedSince(baseDate)(invoice);

        public Func<Invoice, bool> ClosedInvoiceAfter(DateTime baseDate)
            => x => x.CloseDate > baseDate;

        public InvoiceStatus Status(Invoice invoice, DateTime baseDate)
            => invoice switch
            {
                _ when IsPaid(invoice, baseDate) => InvoiceStatus.Paid,
                _ when IsOverdue(invoice, baseDate) => InvoiceStatus.Overdue,
                _ when IsClosed(invoice, baseDate) => InvoiceStatus.Closed,
                _ when IsOpened(invoice, baseDate) => InvoiceStatus.Open,
                _ => InvoiceStatus.Future
            };

        public int DaysRemainingToNextStage(Invoice invoice, DateTime baseDate)
           => Status(invoice, baseDate) switch
           {
               InvoiceStatus.Paid => 0,
               InvoiceStatus.Closed => DaysToOverdue(invoice, baseDate),
               InvoiceStatus.Open => DaysToClose(invoice, baseDate),
               InvoiceStatus.Overdue => OverdueDays(invoice, baseDate),
               _ => DaysToOpen(invoice, baseDate),
           };

        public int DaysToOpen(Invoice invoice, DateTime baseDate)
            => DiffInDays(CurrentInvoiceOpenDate(invoice), baseDate);

        public int OverdueDays(Invoice invoice, DateTime baseDate)
            => DiffInDays(baseDate, invoice.DueDate);

        public int DaysToOverdue(Invoice invoice, DateTime baseDate)
            => DiffInDays(invoice.DueDate, baseDate);

        public int DaysToClose(Invoice invoice, DateTime baseDate)
            => DiffInDays(invoice.CloseDate, baseDate);

        private int DiffInDays(DateTime date1, DateTime date2)
            => (date1.Date - date2.Date).Days;

        public bool IsPaid(Invoice invoice, DateTime baseDate)
            => invoice.PaymentStatus == PaymentStatus.Paid
            && baseDate >= invoice.PaymentDate;

        public bool IsOpened(Invoice invoice, DateTime baseDate)
            => !IsClosed(invoice, baseDate)
            && baseDate.Date >= CurrentInvoiceOpenDate(invoice).Date;

        public bool IsOverdue(Invoice invoice, DateTime baseDate)
            => IsClosed(invoice, baseDate)
            && !IsPaid(invoice, baseDate)
            && invoice.DueDate.Date < baseDate.Date;

        public bool IsClosed(Invoice invoice, DateTime baseDate)
            => invoice.CloseDate.Date < baseDate.Date;

        private DateTime CurrentInvoiceOpenDate(Invoice invoice)
            => GetInvoiceCloseDateBy(invoice.CloseDate.AddMonths(-1)).AddDays(1);
    }
}
