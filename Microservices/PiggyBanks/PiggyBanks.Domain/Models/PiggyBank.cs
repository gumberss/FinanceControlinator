using FinanceControlinator.Common.Entities;
using PiggyBanks.Domain.Enums;
using System;
using System.Collections.Generic;

namespace PiggyBanks.Domain.Models
{
    public class PiggyBank : Entity<Guid>
    {
        public PiggyBank()
        {
            SourceTransfers = new List<Transfer>();
            DestinationTransfers = new List<Transfer>();
        }

        public String Title { get; set; }

        public String Description { get; set; }

        public PiggyBankType Type { get; set; }

        public DateTime GoalDate { get; set; }

        public decimal GoalValue { get; set; }

        public decimal SavedValue { get; set; }

        public DateTime StartDate { get; set; }

        public bool Default { get; set; }

        public virtual List<Transfer> DestinationTransfers { get; set; }

        public virtual List<Transfer> SourceTransfers { get; set; }

        public PiggyBank AsDefault()
        {
            Default = true;
            Title = "Default";
            Type = PiggyBankType.Other;

            return this;
        }

        public PiggyBank AddMoney(decimal value)
        {
            SavedValue += value;

            Updated();

            return this;
        }

        public PiggyBank InstallmentPaid(decimal value)
        {
            return AddMoney(value);
        }

        public bool CanWithdraw(decimal amount) => SavedValue >= amount;

        public PiggyBank Withdraw(decimal amount)
        {
            SavedValue -= amount;
            Updated();
            return this;
        }
    }
}
