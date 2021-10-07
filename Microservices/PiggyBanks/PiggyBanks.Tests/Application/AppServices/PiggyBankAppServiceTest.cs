using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using FluentValidation.Results;
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
using System.Net;
using System.Text;
using System.Threading;
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
        private Mock<IPiggyBankValidator> _piggyBankValidatorMock;

        [TestInitialize]
        public void Init()
        {
            _PiggyBankRepositoryMock = new Mock<IPiggyBankRepository>();
            _localizationMock = new Mock<ILocalization>();
            _loggerMock = new Mock<ILogger<IPiggyBankAppService>>();
            _piggyBankValidatorMock = new Mock<IPiggyBankValidator>();

            _appService = new PiggyBankAppService(
                _PiggyBankRepositoryMock.Object,
                _localizationMock.Object,
                _loggerMock.Object,
                _piggyBankValidatorMock.Object
            );
        }

        #region Register PiggyBank

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingPiggyBanks)]
        [UnitTestCategory(TestMicroserviceEnum.PiggyBanks, TestFeatureEnum.PiggyBankGeneration)]
        public async Task Should_return_an_error_when_piggy_bank_has_invalid_properties()
        {
            var errorMessage = "Oh No!";
            var piggyBank = new PiggyBank();

            var validationResult = new ValidationResult(
                new List<ValidationFailure>
                {
                    new ValidationFailure("Property",errorMessage)
                });

            _piggyBankValidatorMock
                .Setup(x => x.ValidateAsync(piggyBank, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var result = await _appService.RegisterPiggyBank(piggyBank);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(HttpStatusCode.BadRequest);
            result.Error.Message.Should().Be(errorMessage);
            _PiggyBankRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PiggyBank>()), Times.Never);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingPiggyBanks)]
        [UnitTestCategory(TestMicroserviceEnum.PiggyBanks, TestFeatureEnum.PiggyBankGeneration)]
        public async Task Should_return_an_error_when_exists_a_piggy_bank_registered_with_the_same_title()
        {
            var piggyBank = new PiggyBank() { Title = "Piggy" };

            var validationResult = new ValidationResult();

            var alreadyExistByTitleMessageError = "PIGGY_BANK_ALREADY_EXISTS_BY_TITLE";

            _localizationMock
                .SetupGet(x => x.PIGGY_BANK_ALREADY_EXISTS_BY_TITLE)
                .Returns(alreadyExistByTitleMessageError);

            _piggyBankValidatorMock
                .Setup(x => x.ValidateAsync(piggyBank, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _PiggyBankRepositoryMock.Setup(x => x.ExistsPiggyBankByTitle(piggyBank.Title))
                .ReturnsAsync(true);

            var result = await _appService.RegisterPiggyBank(piggyBank);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(HttpStatusCode.BadRequest);
            result.Error.Message.Should().Be(alreadyExistByTitleMessageError);
            _PiggyBankRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PiggyBank>()), Times.Never);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingPiggyBanks)]
        [UnitTestCategory(TestMicroserviceEnum.PiggyBanks, TestFeatureEnum.PiggyBankGeneration)]
        public async Task Should_return_an_error_when_an_exception_occurs_retrieving_piggybanks_by_title_from_repository()
        {
            var piggyBank = new PiggyBank() { Title = "Piggy" };

            var errorMessage = "Oh No!";

            var validationResult = new ValidationResult();
            var exception = new BusinessException(HttpStatusCode.InternalServerError, errorMessage);

            _piggyBankValidatorMock
                .Setup(x => x.ValidateAsync(piggyBank, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _PiggyBankRepositoryMock.Setup(x => x.ExistsPiggyBankByTitle(piggyBank.Title))
                .ReturnsAsync(exception);

            var result = await _appService.RegisterPiggyBank(piggyBank);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(HttpStatusCode.InternalServerError);
            result.Error.Message.Should().Be(errorMessage);
            _PiggyBankRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PiggyBank>()), Times.Never);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingPiggyBanks)]
        [UnitTestCategory(TestMicroserviceEnum.PiggyBanks, TestFeatureEnum.PiggyBankGeneration)]
        public async Task Should_register_a_new_piggy_bank_when_received_piggy_bank_is_valid_to_register()
        {
            var piggyBank = new PiggyBank() { Title = "Piggy" };

            _PiggyBankRepositoryMock.Setup(x => x.AddAsync(piggyBank)).ReturnsAsync(piggyBank);

            _piggyBankValidatorMock
                .Setup(x => x.ValidateAsync(piggyBank, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var result = await _appService.RegisterPiggyBank(piggyBank);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().Be(piggyBank);

            _PiggyBankRepositoryMock.Verify(x => x.AddAsync(piggyBank), Times.Once);
        }

        #endregion Register PiggyBank

        #region register paid invoice

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.InvoicePayment)]
        [UnitTestCategory(TestMicroserviceEnum.PiggyBanks, TestFeatureEnum.Payment)]
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
        [JourneyCategory(TestUserJourneyEnum.InvoicePayment)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.Payment)]
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

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.InvoicePayment)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.Payment)]
        public async Task Should_return_an_error_when_an_exception_occurs_retrieving_piggybanks_from_repository()
        {
            var exception = new BusinessException(HttpStatusCode.InternalServerError, "Oh no!");

            _PiggyBankRepositoryMock
                .Setup(x => x.GetAllAsync(null, It.IsAny<Expression<Func<PiggyBank, bool>>>()))
                .ReturnsAsync(exception);

            var changedPiggyBanks = await _appService.RegisterPayment(new Invoice());

            changedPiggyBanks.IsFailure.Should().BeTrue();

            changedPiggyBanks.Error.Should().Be(exception);
        }

        #endregion register paid invoice
    }
}
