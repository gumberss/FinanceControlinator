﻿using Expenses.Domain.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Tests.Domain
{
    [TestClass]
    public class ExpensesTest
    {

        [TestMethod]
        public void Deveria_possuir_o_custo_total_da_espesa_valida_quando_for_igual_a_soma_dos_custos_dos_itens()
        {
            var expense = new Expense()
            {
                TotalCost = 100,
                Items = new List<ExpenseItem>
                {
                    new ExpenseItem(){ Cost = 25, Amount = 2},
                    new ExpenseItem(){ Cost = 50, Amount = 1}
                }
            };

            expense.TotalCostIsValid().Should().BeTrue();
        }
    }
}
