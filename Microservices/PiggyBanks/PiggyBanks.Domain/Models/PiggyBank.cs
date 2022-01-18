using FinanceControlinator.Common.Entities;
using PiggyBanks.Domain.Enums;
using System;

namespace PiggyBanks.Domain.Models
{
    public class PiggyBank : Entity<Guid>
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public PiggyBankType Type { get; set; }

        public DateTime GoalDate { get; set; }

        public decimal GoalValue { get; set; }

        public decimal SavedValue { get; set; }

        public DateTime StartDate { get; set; }

        public bool Default { get; set; }

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
    }
}
