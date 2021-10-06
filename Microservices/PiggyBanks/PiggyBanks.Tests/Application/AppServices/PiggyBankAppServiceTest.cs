﻿using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PiggyBanks.Application.AppServices;
using PiggyBanks.Application.Interfaces.AppServices;
using PiggyBanks.Data.Repositories;
using PiggyBanks.Domain.Interfaces.Validators;
using PiggyBanks.Domain.Localizations;
using PiggyBanks.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBanks.Tests.Application.AppServices
{
    [TestClass]
    public class PiggyBankAppServiceTest
    {
        private IPiggyBankAppService _appService;
        private Mock<IPiggyBankRepository> _PiggyBankRepositoryMock;
        private Mock<ILocalization> _localizationMock;
        private Mock<ILogger<IPiggyBankAppService>> _loggerMock;
        private Mock<IPiggyBankValidator> _piggyBankValidator;

        [TestInitialize]
        public void Init()
        {
            _PiggyBankRepositoryMock = new Mock<IPiggyBankRepository>();
            _localizationMock = new Mock<ILocalization>();
            _loggerMock = new Mock<ILogger<IPiggyBankAppService>>();
            _piggyBankValidator = new Mock<IPiggyBankValidator>();

            _appService = new PiggyBankAppService(
                _PiggyBankRepositoryMock.Object,
                _localizationMock.Object,
                _loggerMock.Object,
                _piggyBankValidator.Object
            );
        }

        [TestMethod]
        public async Task Should_add_installment_cost_to_the_piggybank_when_the_paid_invoice_containts_a_item_regarding_the_piggybank()
        {
            var piggyBankId = Guid.NewGuid();
            var installmentCost = 100;

            var piggyBanksDb = new List<PiggyBank>
            {
                new PiggyBank(){ Id = piggyBankId }
            };

            var invoice = new Invoice();
            invoice.Items.Add(new InvoiceItem() { ExpenseId = piggyBankId, InstallmentCost = installmentCost });

            _PiggyBankRepositoryMock
                .Setup(x => x.GetAllAsync(null, It.IsAny<Expression<Func<PiggyBank, bool>>>()))
                .Returns<Expression<Func<PiggyBank, object>>, Expression<Func<PiggyBank, bool>>[]>((include, wheres)
                    => Result.Try(() =>
                    {
                        var resultData = new List<PiggyBank>(piggyBanksDb);
                        wheres.ToList().ForEach(where => resultData = resultData.Where(where.Compile()).ToList());
                        return resultData;
                    }));

            var changedPiggyBanks = await _appService.RegisterPayment(invoice);

            changedPiggyBanks.IsSuccess.Should().BeTrue();
            changedPiggyBanks.Value.Should().HaveCount(1);
            changedPiggyBanks.Value.First().SavedValue.Should().Be(100);
        }

        [TestMethod]
        public async Task Should_not_add_installment_cost_to_the_piggybank_when_the_paid_invoice_not_containts_a_item_regarding_the_piggybank()
        {
            var piggyBankId = Guid.NewGuid();
            var installmentCost = 100;

            var piggyBanksDb = new List<PiggyBank>
            {
                new PiggyBank(){ Id = Guid.NewGuid(), SavedValue = 0 }
            };

            var invoice = new Invoice();
            invoice.Items.Add(new InvoiceItem() { ExpenseId = piggyBankId, InstallmentCost = installmentCost });

            _PiggyBankRepositoryMock
                .Setup(x => x.GetAllAsync(null, It.IsAny<Expression<Func<PiggyBank, bool>>>()))
                .Returns<Expression<Func<PiggyBank, object>>, Expression<Func<PiggyBank, bool>>[]>((include, wheres)
                    => Result.Try(() =>
                    {
                        var resultData = new List<PiggyBank>(piggyBanksDb);
                        wheres.ToList().ForEach(where => resultData = resultData.Where(where.Compile()).ToList());
                        return resultData;
                    }));

            var changedPiggyBanks = await _appService.RegisterPayment(invoice);

            changedPiggyBanks.IsSuccess.Should().BeTrue();
            changedPiggyBanks.Value.Should().HaveCount(0);

            piggyBanksDb.First().SavedValue.Should().Be(0);
        }
    }
}
