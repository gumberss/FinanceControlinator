using Expenses.Domain.Enums;
using Expenses.Domain.Localizations;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Expenses.Tests.Domain.Localizations
{
    [TestClass]
    public class PtbrTest
    {
        Ptbr _ptbr;
        public PtbrTest()
        {
            _ptbr = new Ptbr();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.ExpenseGeneral)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.General)]
        public void Should_have_an_answer_to_each_expense_type()
        {
            Enum.GetValues(typeof(ExpenseType)).Cast<ExpenseType>()
                .Select(x => _ptbr.EXPENSE_TYPE(x))
                .All(x => x is not null)
                .Should()
                .BeTrue();
        }
    }
}
