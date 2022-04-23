using Expenses.Application.AppServices;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Interfaces.Validators;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Exceptions;
using FinanceControlinator.Common.Utils;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Tests.Application.AppServices
{
    [TestClass]
    public class ExpenseAppServiceTest
    {
        public ExpenseAppService _service;

        readonly Mock<IExpenseRepository> _expenseRepository;
        readonly Mock<IInvoiceRepository> _invoiceRepository;
        readonly Mock<IExpenseValidator> _expenseValidator;
        readonly Mock<ILocalization> _localization;
        readonly Mock<ILogger<IExpenseAppService>> _logger;
        readonly Mock<IExpenseService> _expenseService;

        public ExpenseAppServiceTest()
        {
            _expenseRepository = new Mock<IExpenseRepository>();
            _invoiceRepository = new Mock<IInvoiceRepository>();
            _expenseValidator = new Mock<IExpenseValidator>();
            _localization = new Mock<ILocalization>();
            _logger = new Mock<ILogger<IExpenseAppService>>();
            _expenseService = new Mock<IExpenseService>();

            _service = new ExpenseAppService(
                _expenseRepository.Object,
                _invoiceRepository.Object,
                _expenseValidator.Object,
                _localization.Object,
                _logger.Object,
                _expenseService.Object);
        }

        #region Register expense

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public async Task Should_validate_expense()
        {
            var expense = new Expense { Items = new List<ExpenseItem>() };

            _expenseValidator
                .Setup(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new ValidationResult()));

            await _service.RegisterExpense(expense);

            _expenseValidator.Verify(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public async Task Should_return_an_error_when_expense_is_invalid()
        {
            var error = new ValidationFailure("A prop", "I found a error here");

            var expense = new Expense { Items = new List<ExpenseItem>() };

            _expenseValidator
                .Setup(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new ValidationResult(new[] { error })));

            var result = await _service.RegisterExpense(expense);

            _expenseValidator.Verify(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()), Times.Once);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public async Task Should_log_the_error_as_information_when_expense_is_invalid()
        {
            var error = new ValidationFailure("A prop", "I found a error here");

            var expense = new Expense { Items = new List<ExpenseItem>() };

            _expenseValidator
                .Setup(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new ValidationResult(new[] { error })));

            var result = await _service.RegisterExpense(expense);

            _expenseValidator.Verify(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()), Times.Once);

            _logger.Verify(x => x.Log(LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public async Task Should_not_add_to_repository_when_expense_is_invalid()
        {
            var error = new ValidationFailure("A prop", "I found a error here");

            var expense = new Expense { Items = new List<ExpenseItem>() };

            _expenseValidator
                .Setup(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new ValidationResult(new[] { error })));

            var result = await _service.RegisterExpense(expense);
            result.IsFailure.Should().BeTrue();
            result.Error.Message.Should().Be("I found a error here");
            _expenseValidator.Verify(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()), Times.Once);
            _expenseRepository.Verify(x => x.AddAsync(It.IsAny<Expense>()), Times.Never);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public async Task Should_add_to_repository_when_expense_is_valid()
        {
            var expense = new Expense { Items = new List<ExpenseItem>() };

            _expenseValidator
                .Setup(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new ValidationResult()));

            var _ = await _service.RegisterExpense(expense);

            _expenseRepository.Verify(x => x.AddAsync(expense), Times.Once);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
        public async Task Should_return_an_error_when_add_expense_to_repository_return_an_error()
        {
            var expense = new Expense { Items = new List<ExpenseItem>() };

            var exception = new BusinessException(HttpStatusCode.InternalServerError, new ErrorData("Oh no!"));

            _expenseRepository
                .Setup(x => x.AddAsync(expense))
                .Returns(Task.FromResult(new Result<Expense, BusinessException>(exception)));

            _expenseValidator
                .Setup(x => x.ValidateAsync(expense, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new ValidationResult()));

            var result = await _service.RegisterExpense(expense);

            _expenseRepository.Verify(x => x.AddAsync(expense), Times.Once);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(HttpStatusCode.InternalServerError);
        }

        #endregion Register expense
    }
}
