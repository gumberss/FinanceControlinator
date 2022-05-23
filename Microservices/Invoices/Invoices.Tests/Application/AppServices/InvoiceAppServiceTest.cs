using FinanceControlinator.Common.Utils;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Application.AppServices;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raven.Client.Documents;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Invoices.Tests.Application.AppServices
{
    [TestClass]
    [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
    [IntegrationTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseGeneration)]
    public class InvoiceAppServiceTest
    {
        [TestMethod]
        public async Task Should_found_registered_invoices_when_first_invoice_date_is_in_current_year_and_last_invoice_date_is_in_the_next()
        {
            var closeDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            var invoices = Enumerable.Range(0, 12)
                .Select(x => new Invoice(closeDate.AddMonths(x)))
                .ToList();

            var invoiceServiceMock = new Mock<IInvoiceRepository>();

            invoiceServiceMock
                .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Invoice, object>>>(), It.IsAny<Expression<Func<Invoice, bool>>>()))
                   .Returns<Expression<Func<Invoice, object>>, Expression<Func<Invoice, bool>>[]>((_, wheres)
                    => Result.Try(() =>
                    {
                        wheres.ToList().ForEach(x => invoices = invoices.Where(x.Compile()).ToList());
                        return invoices;
                    }));

            var service = new InvoiceAppService(
                    invoiceServiceMock.Object,
                    new Mock<IExpenseRepository>().Object,
                    new Mock<ILocalization>().Object,
                    new Mock<ILogger<IInvoiceAppService>>().Object,
                    new InvoiceService(),
                    new Mock<IPaymentRepository>().Object
                );

            var result = await service.RegisterInvoiceItems(new Expense
            {
                InstallmentsCount = 12,
                TotalCost = 120,
            });

            result.Value.Except(invoices).Should().HaveCount(0);
        }
    }
}
