using AutoMapper;
using Expenses.Domain.Models.Expenses.Overviews;
using Expenses.Handler.Configurations.Profiles;
using Expenses.Handler.Domain.Cqrs.ExpenseOverviews;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Expenses.Tests.Handler.Profiles
{
    [TestClass]
    public class OverviewProfileTest
    {
        private readonly MapperConfiguration _config;

        public OverviewProfileTest()
        {
            _config = new MapperConfiguration(cfg => cfg.AddProfile<OverviewProfile>());
        }

        [TestMethod]
        public void Is_valid_configuration()
        {
            _config.AssertConfigurationIsValid();
        }

        [TestMethod]
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
                new ExpensePartition("Market", 40.25f, 1080.32M),
                new ExpensePartition("Bill", 21.25f, 180.32M)
            });

            var expenseOverviewDTO = new ExpenseOverviewDTO
            {
                Briefs = new List<ExpenseBriefDTO>
                {
                    new ExpenseBriefDTO { Text = "A brief" },
                    new ExpenseBriefDTO { Text = "A brief 2" }
                },
                ExpensePartitions = new List<ExpensePartitionDTO>
                {
                    new ExpensePartitionDTO
                    {
                        Type = "Market", 
                        Percent = 40.25f, 
                        TotalValue = 1080.32M
                    },
                    new ExpensePartitionDTO
                    {
                        Type = "Bill", 
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
