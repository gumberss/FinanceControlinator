﻿using FinanceControlinator.Common.Entities;
using System;

namespace Expenses.Domain.Models
{
    public class ExpenseItem : Entity
    {
        public String Name { get; init; }

        public String Description { get; init; }

        public decimal Cost { get; set; }

        public int Amount { get; init; }

        public Guid ExpenseId { get; set; }

        public Expense Expense { get; init; }
    }
}
