﻿using FinanceControlinator.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PiggyBanks.Domain.Models
{
    public class Invoice : Entity<Guid>
    {
        public Invoice()
        {
            Items = new List<InvoiceItem>();
        }

        public DateTime DueDate { get; private set; }

        public virtual List<InvoiceItem> Items { get; private set; }

        public IEnumerable<InvoiceItem> ItemsFrom(PiggyBank piggyBank)
        {
            return Items.Where(x => x.ExpenseId == piggyBank.Id);
        }

        public Invoice ChangeDueDate(DateTime dueDate)
        {
            DueDate = dueDate;

            return this;
        }

        public Invoice ReplaceItems(List<InvoiceItem> newItems)
        {
            Items.Clear();
            Items.AddRange(newItems);
            newItems.ForEach(x => x.LinkTo(this));

            Updated();

            return this;
        }
    }
}
