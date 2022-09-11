using FinanceControlinator.Common.Parsers.TextParsers;
using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Localizations;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Invoices.Tests.Domain.Services
{
    [TestClass]
    public class InvoiceSyncServiceTest
    {
        private readonly InvoiceSyncService _invoiceSyncService;
        private readonly Mock<ILocalization> _localization;
        private readonly Mock<ITextParser> _textParser;

        public InvoiceSyncServiceTest()
        {
            _invoiceSyncService = new InvoiceSyncService();
            _localization = new Mock<ILocalization>();
            _textParser = new Mock<ITextParser>();

            _textParser.Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<(string, string)>()))
                .Returns(new TextParser().Parse);

            var realLocalziation = new Ptbr();
            _localization.SetupGet(x => x.INVOICE_INSTALLMENT_NUMBER).Returns(realLocalziation.INVOICE_INSTALLMENT_NUMBER);
            _localization.SetupGet(x => x.CULTURE).Returns(realLocalziation.CULTURE);
            _localization.Setup(x => x.FORMAT_MONEY(It.IsAny<decimal>())).Returns(realLocalziation.FORMAT_MONEY);
        }

        [TestMethod]
        public void Should_build_invoice_sync()
        {
            var closeDate = new DateTime(2022, 06, 03, 10, 15, 02);
            var invoice = new Invoice(closeDate);
            invoice.AddNew(new InvoiceItem(1, 2)
            .From(new Expense
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Title",
                Type = InvoiceItemType.Investment,
                PurchaseDay = new DateTime(2022, 06, 02, 10, 15, 02)
            }));

            var invoiceSync = _invoiceSyncService.BuildInvoiceSync(invoice, _localization.Object, _textParser.Object);

            invoiceSync.Id.Should().Be(invoice.Id);
            invoiceSync.TotalCost.Should().Be("R$ 2,00");
            invoiceSync.CloseDate.Should().Be("03/06/2022");
            invoiceSync.DueDate.Should().Be("10/06/2022");
            invoiceSync.PaymentDate.Should().BeEmpty();
            invoiceSync.PaymentStatus.Should().Be(PaymentStatus.Opened);

            invoiceSync.Items.Should().HaveCount(1);

        }

        [TestMethod]
        public void Should_build_invoice_item_sync()
        {
            var invoiceItem = new InvoiceItem(1, 2)
            .From(new Expense
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Title",
                Type = InvoiceItemType.Investment,
                PurchaseDay = new DateTime(2022, 06, 02, 10, 15, 02)
            });

            var invoiceItemSync = _invoiceSyncService.BuildInvoiceItemSync(invoiceItem, _localization.Object, _textParser.Object);

            invoiceItemSync.Id.Should().Be(invoiceItem.Id);
            invoiceItemSync.Title.Should().Be(invoiceItem.Title);
            invoiceItemSync.InstallmentNumber.Should().Be("Parcela: 1");
            invoiceItemSync.InstallmentCost.Should().Be("R$ 2,00");
            invoiceItemSync.Type.Should().Be(InvoiceItemType.Investment);
            invoiceItemSync.PurchaseDay.Should().Be("2 de junho - 10:15");
        }
    }
}
