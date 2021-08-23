using FinanceControlinator.Common.Utils;
using FluentAssertions;
using Invoices.Application.AppServices;
using Invoices.Application.Interfaces.AppServices;
using Invoices.Data.Repositories;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Tests.Application.AppServices
{
    [TestClass]
    public class InvoiceServiceTest
    {
        private IInvoiceRepository invoiceServiceMock;

        [TestMethod]
        [TestCategory("Integration")]
        public async Task Should_found_registered_invoices_when_first_invoice_date_is_in_current_year_and_last_invoice_date_is_in_the_next()
        {
            var closeDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            var invoices = Enumerable.Range(0, 12)
                .Select(x => new Invoice(closeDate.AddMonths(x)))
                .ToList();

            invoiceServiceMock = Substitute.For<IInvoiceRepository>();

            invoiceServiceMock
                .GetAllAsync(Arg.Any<Expression<Func<Invoice, object>>>(), Arg.Any<Expression<Func<Invoice, bool>>>())
                .Returns(x =>
                {
                    var wheres = x.Arg<Expression<Func<Invoice, bool>>[]>().ToList();

                    wheres.ForEach(x => invoices = invoices.Where(x.Compile()).ToList());

                    return invoices;
                });

            var service = new InvoiceAppService(
                    invoiceServiceMock,
                    Substitute.For<IExpenseRepository>(),
                    Substitute.For<ILocalization>(),
                    Substitute.For<ILogger<IInvoiceAppService>>(),
                    new InvoiceService() // maybe this is not right...
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
