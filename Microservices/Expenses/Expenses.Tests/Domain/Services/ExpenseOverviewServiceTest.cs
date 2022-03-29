using Expenses.Domain.Enums;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Services;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Tests.Domain.Services
{
    [TestClass]
    public class ExpenseOverviewServiceTest
    {
        private readonly ExpenseOverviewService _service;

        public ExpenseOverviewServiceTest()
        {
            _service = new ExpenseOverviewService();
        }

        #region MostSpentMoneyPlace

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_the_expense_location_when_there_is_only_one_expense()
        {
            var expenses = new List<Expense>
            {
                new Expense
                {
                    Location = "A beautiful place",
                    TotalCost = 100
                }
            };

            var result = _service.MostSpentMoneyPlace(expenses);

            result.Should().Be(("A beautiful place", 100));
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_not_return_a_place_when_no_expense_was_made()
        {
            var expenses = new List<Expense>();

            var result = _service.MostSpentMoneyPlace(expenses);

            result.local.Should().BeNull();
            result.totalSpendMoney.Should().Be(0);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_the_expense_location_of_themost_spent_place_when_there_are_more_than_one_expense()
        {
            var expenses = new List<Expense>
            {
                new Expense
                {
                    Location = "A beautiful place",
                    TotalCost = 100
                },
                 new Expense
                {
                    Location = "An other place",
                    TotalCost = 50
                }
            };

            var result = _service.MostSpentMoneyPlace(expenses);

            result.Should().Be(("A beautiful place", 100));
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_the_expense_location_when_the_last_expense_is_expensive()
        {
            var expenses = new List<Expense>
            {
                new Expense
                {
                    Location = "A beautiful place",
                    TotalCost = 50
                },
                new Expense
                {
                    Location = "An other place",
                    TotalCost = 100
                }
            };

            var result = _service.MostSpentMoneyPlace(expenses);

            result.Should().Be(("An other place", 100));
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_the_place_with_the_sum_of_expenses_in_this_place_is_the_bigger()
        {
            var expenses = new List<Expense>
            {
                new Expense
                {
                    Location = "A beautiful place",
                    TotalCost = 70.03M
                },
                new Expense
                {
                    Location = "A beautiful place",
                    TotalCost = 40.21M
                },
                new Expense
                {
                    Location = "An other place",
                    TotalCost = 100.32M
                }
            };

            var result = _service.MostSpentMoneyPlace(expenses);

            result.Should().Be(("A beautiful place", 110.24M));
        }

        #endregion MostSpentMoneyPlace

        #region MostSpentType

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_null_when_there_are_no_expense()
        {
            var expenses = new List<Expense>();

            var result = _service.MostSpentType(expenses);

            result.Should().BeNull();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_the_expense_type_when_there_is_only_one_expense()
        {
            var expenses = new List<Expense>
            {
                  new Expense
                {
                    Type = ExpenseType.Health,
                    TotalCost = 70
                },
            };

            var result = _service.MostSpentType(expenses);

            result.Should().Be(ExpenseType.Health);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_the_expense_type_with_the_sum_of_expenses_of_it_is_the_bigger()
        {
            var expenses = new List<Expense>
            {
                  new Expense
                {
                    Type = ExpenseType.Health,
                    TotalCost = 70
                },
                new Expense
                {
                    Type = ExpenseType.Health,
                    TotalCost = 70
                },
                new Expense
                {
                    Type = ExpenseType.Bill,
                    TotalCost = 130
                },
                new Expense
                {
                    Type = ExpenseType.Market,
                    TotalCost = 71
                },
                new Expense
                {
                    Type = ExpenseType.Market,
                    TotalCost = 72
                },
            };

            var result = _service.MostSpentType(expenses);

            result.Should().Be(ExpenseType.Market);
        }

        #endregion MostSpentType

        #region TotalMoneySpent

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_zero_when_there_are_no_expense()
        {
            var expenses = new List<Expense>();

            var result = _service.TotalMoneySpent(expenses);

            result.Should().Be(0);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_the_sum_of_all_expenses_when_there_are_expenses()
        {
            var expenses = new List<Expense>
            {
                new Expense { TotalCost = 1.10M },
                new Expense { TotalCost = 2.20M },
                new Expense { TotalCost = 3.30M },
                new Expense { TotalCost = 4.40M },
            };

            var result = _service.TotalMoneySpent(expenses);

            result.Should().Be(11.00M);
        }

        #endregion TotalMoneySpent

        #region GroupByType

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_return_a_partition_for_each_exppense_type_even_though_there_are_no_spend_of_the_type()
        {
            var partitions = _service.GroupByType(new List<Expense>());

            var expenseTypes = Enum.GetValues(typeof(ExpenseType)).Cast<ExpenseType>();

            partitions.Should().HaveCount(expenseTypes.Count());

            expenseTypes.All(expenseType => partitions.Any(y => y.Type == expenseType));
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_make_partitions_grouping_expenses_by_type()
        {
            var expenses = new List<Expense>
            {
                new Expense
                {
                    Type = ExpenseType.Market,
                    TotalCost = 700
                },
                new Expense
                {
                    Type = ExpenseType.Market,
                    TotalCost = 200
                },
                new Expense
                {
                    Type = ExpenseType.Leisure,
                    TotalCost = 100
                }
            };

            var partitions = _service.GroupByType(expenses);

            var marketPartition = partitions.Find(x => x.Type == ExpenseType.Market);

            marketPartition.TotalValue.Should().Be(900);
            marketPartition.Percent.Should().Be(90);

            var leisurePartition = partitions.Find(x => x.Type == ExpenseType.Leisure);
            leisurePartition.TotalValue.Should().Be(100);
            leisurePartition.Percent.Should().Be(10);
        }

        #endregion GroupByType
    }
}
