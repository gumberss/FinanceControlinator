using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Expenses.Tests.Domain.Models
{
    [TestClass]
    public class ExpensesTest
    {

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public void Deveria_possuir_o_custo_total_da_despesa_valida_quando_for_igual_a_soma_dos_custos_dos_itens()
        {
            var expense = new Expense()
            {
                TotalCost = 100,
                Items = new List<ExpenseItem>
                {
                    new ExpenseItem(){ Cost = 25, Amount = 2 },
                    new ExpenseItem(){ Cost = 50, Amount = 1 }
                }
            };

            expense.TotalCostIsValid().Should().BeTrue();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public void Deveria_possuir_o_custo_total_da_despesa_invalido_quando_for_diferente_da_soma_dos_custos_dos_itens()
        {
            var expense = new Expense()
            {
                TotalCost = 100,
                Items = new List<ExpenseItem>
                {
                    new ExpenseItem(){ Cost = 10, Amount = 2 },
                    new ExpenseItem(){ Cost = 10, Amount = 1 }
                }
            };

            expense.TotalCostIsValid().Should().BeFalse();
        }
    }
}
