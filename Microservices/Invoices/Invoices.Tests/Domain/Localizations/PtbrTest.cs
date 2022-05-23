using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Localizations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Invoices.Tests.Domain.Localizations
{
    [TestClass]
    public class PtbrTest
    {
        readonly Ptbr _ptbr;
        public PtbrTest() => _ptbr = new Ptbr();

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.Localization)]
        [UnitTestCategory(TestMicroserviceEnum.Localization, TestFeatureEnum.General)]
        public void Should_have_an_answer_to_each_expense_type()
            => Enum.GetValues(typeof(InvoiceItemType)).Cast<InvoiceItemType>()
                .Select(x => _ptbr.INVOICE_ITEM_TYPE(x))
                .All(x => x is not null)
                .Should()
                .BeTrue();
    }
}
