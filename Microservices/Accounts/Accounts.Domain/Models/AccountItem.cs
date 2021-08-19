using FinanceControlinator.Common.Entities;
using Accounts.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounts.Domain.Models
{
    public class AccountItem : Entity<String>
    {
        public AccountItem()
        {
            AccountIds = new List<string>();
        }

        public String Title { get; private set; }

        public decimal TotalCost { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime CloseDate { get; private set; }

        public String DetailsPath { get; private set; }

        public AccountStatus AccountStatus { get; set; }

        public List<String> AccountIds { get; private set; }

        public bool CanUpdate() => CloseDate < DateTime.Now;

        public AccountItem UpdateFrom(AccountItem accountItem)
        {
            Title = accountItem.Title;
            TotalCost = accountItem.TotalCost;
            DueDate = accountItem.DueDate;
            CloseDate = accountItem.CloseDate;
            DetailsPath = accountItem.DetailsPath;

            return this;
        }

        public bool CanBePaid() => CloseDate < DateTime.Now;

        public void AddAccountRequest(Account account)
        {
            AccountStatus = account.Status;
            AccountIds.Add(account.Id);
        }
    }
}
