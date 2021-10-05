using Invoices.Domain.Enums;
using FinanceControlinator.Common.Entities;
using System;
using Invoices.Domain.DTOs;

namespace Invoices.Domain.Models
{
    public class Expense : Entity<String>
    {
        public String Title { get; set; }

        public DateTime PurchaseDay { get; set; }

        public InvoiceItemType Type { get; set; }

        public int InstallmentsCount { get; set; }

        public String Location { get; set; }

        public decimal TotalCost { get; set; }

        public String DetailsPath { get; set; }

        public Expense From(InvoicePiggyBank piggyBank)
        {
            Id = piggyBank.Id.ToString();
            Title = piggyBank.Title;
            CreatedDate = DateTime.Now;
            DetailsPath = $"piggybanks/{piggyBank.Id}";
            Location = "Piggy Bank";
            PurchaseDay = piggyBank.CreatedDate;
            Type = InvoiceItemType.PiggyBank;
            TotalCost = piggyBank.GoalValue - piggyBank.SavedValue;

            return this;
        }

        public Expense WithInstallmentCount(int installmentCount)
        {
            InstallmentsCount = installmentCount;

            return this;
        }
    }
}
