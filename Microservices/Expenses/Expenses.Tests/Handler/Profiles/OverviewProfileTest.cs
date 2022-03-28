using AutoMapper;
using Expenses.Domain.Enums;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses.Overviews;
using Expenses.Handler.Configurations.Profiles;
using Expenses.Handler.Domain.Cqrs.ExpenseOverviews;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Expenses.Tests.Handler.Profiles
{
    [TestClass]
    public class OverviewProfileTest
    {
        private readonly MapperConfiguration _config;

        public OverviewProfileTest()
        {
            Mock<IServiceProvider> _serviceProvider = new Mock<IServiceProvider>();
            _serviceProvider.Setup(x => x.GetService(typeof(ILocalization))).Returns(new Ptbr());

            _config = new MapperConfiguration(cfg => cfg.AddProfile(new OverviewProfile(_serviceProvider.Object)));
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Is_valid_configuration()
        {
            _config.AssertConfigurationIsValid();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Overview)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.Overview)]
        public void Should_parse_to_DTO_correct()
        {
            var mapper = _config.CreateMapper();

            var expenseOverview = new ExpenseOverview(new List<ExpenseBrief>
            {
                new ExpenseBrief("A brief"),
                new ExpenseBrief("A brief 2")
            },
            new List<ExpensePartition>
            {
                new ExpensePartition(ExpenseType.Market, 40.25f, 1080.32M),
                new ExpensePartition(ExpenseType.Bill, 21.25f, 180.32M)
            });

            var expenseOverviewDTO = new ExpenseOverviewDTO
            {
                Briefs = new List<ExpenseBriefDTO>
                {
                    new ExpenseBriefDTO { Text = "A brief" },
                    new ExpenseBriefDTO { Text = "A brief 2" }
                },
                Partitions = new List<ExpensePartitionDTO>
                {
                    new ExpensePartitionDTO
                    {
                        Type = "Mercado",
                        Percent = 40.25f,
                        TotalValue = 1080.32M
                    },
                    new ExpensePartitionDTO
                    {
                        Type = "Contas",
                        Percent = 21.25f,
                        TotalValue = 180.32M
                    }
                }
            };

            var result = mapper.Map<ExpenseOverviewDTO>(expenseOverview);

            result.Should().BeEquivalentTo(expenseOverviewDTO);
        }
    }
}
