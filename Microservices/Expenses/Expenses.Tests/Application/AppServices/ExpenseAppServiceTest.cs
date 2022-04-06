using Expenses.Application.AppServices;
using Expenses.Application.Interfaces.AppServices;
using Expenses.Data.Repositories;
using Expenses.Domain.Interfaces.Services;
using Expenses.Domain.Interfaces.Validators;
using Expenses.Domain.Localizations;
using Expenses.Domain.Models.Expenses;
using FinanceControlinator.Common.Utils;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Tests.Application.AppServices
{
    [TestClass]
    public class ExpenseAppServiceTest
    {
        public ExpenseAppService _service { get; }

        readonly Mock<IExpenseRepository> _expenseRepository;
        readonly Mock<IInvoiceRepository> _invoiceRepository;
        readonly Mock<IExpenseItemRepository> _expenseItemRepository;
        readonly Mock<IExpenseValidator> _expenseValidator;
        readonly Mock<ILocalization> _localization;
        readonly Mock<ILogger<IExpenseAppService>> _logger;
        readonly Mock<IExpenseService> _expenseService;
        readonly Mock<IDateService> _dateService;


        public ExpenseAppServiceTest()
        {
            _expenseRepository = new Mock<IExpenseRepository>();
            _invoiceRepository = new Mock<IInvoiceRepository>();
            _expenseItemRepository = new Mock<IExpenseItemRepository>();
            _expenseValidator = new Mock<IExpenseValidator>();
            _localization = new Mock<ILocalization>();
            _logger = new Mock<ILogger<IExpenseAppService>>();
            _expenseService = new Mock<IExpenseService>();
            _dateService = new Mock<IDateService>();


            _service = new ExpenseAppService(
                _expenseRepository.Object,
                _invoiceRepository.Object,
                _expenseValidator.Object,
                _expenseItemRepository.Object,
                _localization.Object,
                _logger.Object,
                _expenseService.Object,
                _dateService.Object);
        }

        #region Register expense

        [TestMethod]
        public async Task Should_validate_expense()
        {
           
        }

        [TestMethod]
        public void Should_return_an_error_when_expense_is_invalid()
        {

        }

        [TestMethod]
        public void Should_verify_if_total_cost_is_valid()
        {

        }

        [TestMethod]
        public void Should_return_an_error_when_total_cost_is_invalid()
        {

        }

        [TestMethod]
        public void Should_not_add_to_repository_when_expense_is_invalid()
        {

        }

        [TestMethod]
        public void Should_not_add_to_repository_when_total_cost_not_is_valid()
        {

        }

        [TestMethod]
        public void Should_add_to_repository_when_expense_is_valid()
        {

        }

        #endregion Register expense
    }
}
