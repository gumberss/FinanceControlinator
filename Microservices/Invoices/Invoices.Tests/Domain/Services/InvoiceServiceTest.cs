using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Invoices.Domain.Enums;
using Invoices.Domain.Models;
using Invoices.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Invoices.Tests.Domain.Services
{
    [TestClass]
    [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
    [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
    public class InvoiceServiceTest
    {
        private static InvoiceService _invoiceService;
        private static DateTime _purchaseDay;
        private Expense _expense;

        static string _dateFormat;
        static CultureInfo _ptbr;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _invoiceService = new InvoiceService();
            _purchaseDay = new DateTime(2021, 08, 05);
            _dateFormat = "dd/MM/yyyy";
            _ptbr = new CultureInfo("pt-BR");
        }

        [TestInitialize]
        public void Init()
        {
            _expense = new Expense
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Expense",
                InstallmentsCount = 2,
                TotalCost = 100,
                Location = "Location",
                PurchaseDay = _purchaseDay,
                Type = InvoiceItemType.Investment
            };
        }

        [TestMethod]
        public void Should_create_a_new_invoice_when_a_new_expense_was_informed()
        {
            _expense.InstallmentsCount = 1;
            var existentInvoices = new List<Invoice>();

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(1);
            invoices.First().TotalCost.Should().Be(100);
            invoices.First().Items.Should().HaveCount(1);

            var invoiceItemFromExpense = invoices.First().Items.First();

            invoiceItemFromExpense.InstallmentCost.Should().Be(_expense.TotalCost, because: "The expense has only one installment");
            invoiceItemFromExpense.ExpenseId.Should().Be(_expense.Id);
            invoiceItemFromExpense.Title.Should().Be(_expense.Title);
        }

        [TestMethod]
        public void Should_create_two_new_invoices_when_the_informed_expense_was_buy_in_two_installments()
        {
            var existentInvoices = new List<Invoice>();

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(2);
            invoices.First().TotalCost.Should().Be(50);
            invoices.First().Items.Should().HaveCount(1);

            invoices.Last().TotalCost.Should().Be(50);
            invoices.Last().Items.Should().HaveCount(1);

            var firstInvoiceItemFromExpense = invoices.First().Items.First();

            firstInvoiceItemFromExpense.InstallmentCost.Should().Be(50);
            firstInvoiceItemFromExpense.ExpenseId.Should().Be(_expense.Id);
            firstInvoiceItemFromExpense.Title.Should().Be(_expense.Title);
            firstInvoiceItemFromExpense.Location.Should().Be(_expense.Location);
            firstInvoiceItemFromExpense.PurchaseDay.Should().Be(_expense.PurchaseDay);
            firstInvoiceItemFromExpense.Type.Should().Be(_expense.Type);

            var secondInvoiceItemFromExpense = invoices.Last().Items.First();

            secondInvoiceItemFromExpense.InstallmentCost.Should().Be(50);
            secondInvoiceItemFromExpense.ExpenseId.Should().Be(_expense.Id);
            secondInvoiceItemFromExpense.Title.Should().Be(_expense.Title);
            secondInvoiceItemFromExpense.Location.Should().Be(_expense.Location);
            secondInvoiceItemFromExpense.PurchaseDay.Should().Be(_expense.PurchaseDay);
            secondInvoiceItemFromExpense.Type.Should().Be(_expense.Type);
        }

        [TestMethod]
        public void Should_add_the_expense_in_an_invoice_when_that_month_invoice_already_exists()
        {
            var purchaseDay = new DateTime(2021, 08, 05);

            _expense.PurchaseDay = purchaseDay;

            var existentInvoice = new Invoice(closeDate: purchaseDay)
            {
                Id = Guid.NewGuid().ToString(),
            };

            existentInvoice.AddNew(new InvoiceItem(1, 100));

            var existentInvoices = new List<Invoice>() { existentInvoice };

            var currentInvoiceDate = purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(2);

            var newInvoices = invoices.Except(existentInvoices);
            newInvoices.Should().HaveCount(1);
            var newInvoice = newInvoices.First();
            newInvoice.Items.Should().HaveCount(1);

            var newInvoiceItem = newInvoices.First().Items.First();
            newInvoiceItem.InstallmentCost.Should().Be(50);
            newInvoiceItem.ExpenseId.Should().Be(_expense.Id);
            newInvoiceItem.Title.Should().Be(_expense.Title);
            newInvoiceItem.Location.Should().Be(_expense.Location);
            newInvoiceItem.PurchaseDay.Should().Be(_expense.PurchaseDay);
            newInvoiceItem.Type.Should().Be(_expense.Type);

            existentInvoice.Items.Should().HaveCount(2, because: "First must be the existent item, the second one must be the new item from new expense");

            var existentInvoiceItem = existentInvoice.Items.Last();
            existentInvoiceItem.InstallmentCost.Should().Be(50);
            existentInvoiceItem.ExpenseId.Should().Be(_expense.Id);
            existentInvoiceItem.Title.Should().Be(_expense.Title);
            existentInvoiceItem.Location.Should().Be(_expense.Location);
            existentInvoiceItem.PurchaseDay.Should().Be(_expense.PurchaseDay);
            existentInvoiceItem.Type.Should().Be(_expense.Type);
        }

        [TestMethod]
        public void Should_create_a_new_invoice_when_the_month_invoice_already_exist_but_of_the_other_year()
        {
            var existentInvoiceDate = new DateTime(2004, 08, 05);

            var existentInvoice = new Invoice(closeDate: existentInvoiceDate)
            {
                Id = Guid.NewGuid().ToString()
            };
            existentInvoice.AddNew(new InvoiceItem(1, 100));

            var existentInvoices = new List<Invoice> { existentInvoice };

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(2);

            invoices.Except(existentInvoices).Should().HaveCount(2);
        }

        [TestMethod]
        public void Should_put_the_rest_of_the_expense_cost_on_the_last_invoice()
        {
            _expense.InstallmentsCount = 3;
            _expense.TotalCost = 100; // 100/3 = 33.33 * 3 = 99.99 + .01 -> rest

            var existentInvoices = new List<Invoice>();

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(3);

            invoices.First().TotalCost.Should().Be(33.33m);
            invoices.First().Items.First().InstallmentCost.Should().Be(33.33m);

            invoices[1].TotalCost.Should().Be(33.33m);
            invoices[1].Items.First().InstallmentCost.Should().Be(33.33m);

            invoices.Last().TotalCost.Should().Be(33.34m, because: "Adding the rest of the expense cost");
            invoices.Last().Items.First().InstallmentCost.Should().Be(33.34m, because: "Adding the rest of the expense cost");
        }

        [TestMethod]
        [DataRow(2021, 9, 1, 2021, 9, 30)]
        [DataRow(2021, 9, 15, 2021, 9, 30)]
        [DataRow(2021, 9, 30, 2021, 9, 30)]
        [DataRow(2021, 10, 30, 2021, 10, 31)]
        public void Should_find_the_correct_close_date_by_a_base_date(
            int baseYear, int baseMonth, int baseDay
          , int closeYear, int closeMonth, int closeDay)
        {
            var baseDate = new DateTime(baseYear, baseMonth, baseDay);
            var invoiceCloseDate = _invoiceService.GetInvoiceCloseDateBy(baseDate);

            invoiceCloseDate.Year.Should().Be(closeYear);
            invoiceCloseDate.Month.Should().Be(closeMonth);
            invoiceCloseDate.Day.Should().Be(closeDay);
        }

        [TestMethod]
        [DataRow("01/09/2021", "01/09/2021", "31/10/2021", 1)]
        [DataRow("15/09/2021", "15/09/2021", "30/11/2021", 2)]
        [DataRow("30/09/2021", "30/09/2021", "31/12/2021", 3)]
        [DataRow("15/10/2021", "15/10/2021", "28/02/2022", 4)]
        public void Should_return_the_correct_range_date_when_a_base_date_and_a_installment_count_are_provided(
        string baseDateString, string startDateString, string endDateString, int installmentsCount)
        {
            string dateFormat = "dd/MM/yyyy";

            CultureInfo ptbr = new CultureInfo("pt-BR");

            var baseDate = DateTime.ParseExact(baseDateString, dateFormat, ptbr);
            var startDate = DateTime.ParseExact(startDateString, dateFormat, ptbr);
            var endDate = DateTime.ParseExact(endDateString, dateFormat, ptbr);

            var (invoiceStartDate, invoiceEndDate) = _invoiceService.GetInvoiceDateRangeByInstallments(installmentsCount, baseDate);

            invoiceStartDate.Should().BeSameDateAs(startDate);
            invoiceEndDate.Should().BeSameDateAs(endDate);
        }

        [TestMethod]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_maior_que_o_dia_da_data_inicial_de_meses_diferente()
        {
            var startDate = new DateTime(2021, 09, 10);
            var endDate = new DateTime(2021, 12, 20);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(3);
        }

        [TestMethod]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_maior_que_o_dia_da_data_inicial_do_mesmo_mes()
        {
            var startDate = new DateTime(2021, 09, 20);
            var endDate = new DateTime(2021, 09, 30);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(1);
        }

        [TestMethod]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_menor_que_o_dia_da_data_inicial_de_meses_diferente()
        {
            var startDate = new DateTime(2021, 09, 20);
            var endDate = new DateTime(2021, 12, 10);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(3);
        }

        [TestMethod]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_menor_que_o_dia_da_data_inicial_do_mesmo_mes()
        {
            var startDate = new DateTime(2021, 09, 10);
            var endDate = new DateTime(2021, 09, 20);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(0);
        }

        [TestMethod]
        [DataRow("10/09/2021", "09/09/2021", 0)]//not complete a month
        [DataRow("10/10/2021", "11/09/2021", 0)]//end date before start date
        [DataRow("10/10/2021", "11/11/2020", 0)]//end day and month after but year before the start date
        [DataRow("10/10/2020", "11/11/2021", 13)]//end day, month and year after the start date
        public void Should_find_correctly_the_installment_count
            (String startDateString, String endDateString, int expected)
        {
            string dateFormat = "dd/MM/yyyy";

            CultureInfo ptbr = new CultureInfo("pt-BR");

            var startDate = DateTime.ParseExact(startDateString, dateFormat, ptbr);
            var endDate = DateTime.ParseExact(endDateString, dateFormat, ptbr);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(expected);
        }

        [TestMethod]
        public void Should_return_the_last_invoices_before_the_invoice_base_when_there_are_more_invoices_than_requested()
        {
            var baseInvoice = new Invoice(DateTime.UtcNow);

            var expected = new List<Invoice> {
                new Invoice(DateTime.UtcNow.AddMonths(-1)),
                new Invoice(DateTime.UtcNow.AddMonths(-2))
            };

            var lastInvoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow.AddMonths(-3)),
                new Invoice(DateTime.UtcNow.AddMonths(-4)),
                new Invoice(DateTime.UtcNow.AddMonths(-5)),
            }
            .Concat(expected)
            .ToList();

            _invoiceService.LastInvoicesFrom(baseInvoice, lastInvoices, 2)
                .Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Should_return_the_last_invoices_before_the_invoice_base_when_there_are_same_invoices_quantity_than_requested()
        {
            var baseInvoice = new Invoice(DateTime.UtcNow);

            var lastInvoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow.AddMonths(-1)),
                new Invoice(DateTime.UtcNow.AddMonths(-2)),
                new Invoice(DateTime.UtcNow.AddMonths(-3)),
                new Invoice(DateTime.UtcNow.AddMonths(-4)),
                new Invoice(DateTime.UtcNow.AddMonths(-5)),
            };

            _invoiceService.LastInvoicesFrom(baseInvoice, lastInvoices, 5)
                .Should().BeEquivalentTo(lastInvoices);
        }

        [TestMethod]
        public void Should_return_the_last_invoices_before_the_invoice_base_when_there_are_les_invoices_than_requested()
        {
            var baseInvoice = new Invoice(DateTime.UtcNow);

            var lastInvoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow.AddMonths(-1)),
                new Invoice(DateTime.UtcNow.AddMonths(-2)),
            };

            _invoiceService.LastInvoicesFrom(baseInvoice, lastInvoices, 5000)
                .Should().BeEquivalentTo(lastInvoices);
        }

        [TestMethod]
        public void Should_have_itens_added_after_the_date_informed()
        {
            var baseDate = DateTime.UtcNow.AddMonths(-4);

            var invoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow.AddMonths(-1000))
                .AddNew(new InvoiceItem(0,0){ CreatedDate = DateTime.UtcNow.AddMonths(-1000)})
                .AddNew(new InvoiceItem(0,0){ CreatedDate = DateTime.UtcNow.AddMonths(-1)}),
                new Invoice(DateTime.UtcNow.AddMonths(-3)).AddNew(new InvoiceItem(0,0){ CreatedDate = DateTime.UtcNow.AddMonths(-1)}),
            };

            invoices
                .All(_invoiceService.AnyItemChangedSince(baseDate))
                .Should().BeTrue();
        }

        [TestMethod]
        public void Should_have_itens_updated_after_the_date_informed()
        {
            var baseDate = DateTime.UtcNow.AddMonths(-4);

            var invoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow.AddMonths(-1000))
                    .AddNew(new InvoiceItem(0,0){
                        UpdatedDate = DateTime.UtcNow.AddMonths(-1000),
                        CreatedDate = DateTime.UtcNow.AddMonths(-500)})
                    .AddNew(new InvoiceItem(0,0){
                        UpdatedDate = DateTime.UtcNow.AddMonths(-1), // Updated recently
                        CreatedDate = DateTime.UtcNow.AddMonths(-500)}),
                new Invoice(DateTime.UtcNow.AddMonths(-2))
                    .AddNew(new InvoiceItem(0,0){
                        UpdatedDate = DateTime.UtcNow.AddMonths(-1),
                        CreatedDate = DateTime.UtcNow.AddMonths(-500)}),
                new Invoice(DateTime.UtcNow.AddMonths(-3))
                    .AddNew(new InvoiceItem(0,0){
                        UpdatedDate = DateTime.UtcNow.AddMonths(-1),
                        CreatedDate = DateTime.UtcNow.AddMonths(-500)}),
            };

            invoices
              .All(_invoiceService.AnyItemChangedSince(baseDate))
              .Should().BeTrue();
        }

        [TestMethod]
        public void Should_not_have_itens_updated_after_the_date_informed_when_no_one_items_was_changed_after_the_date_informed()
        {
            var baseDate = DateTime.UtcNow.AddMonths(-4);

            var invoices = new List<Invoice>
            {
                new Invoice(DateTime.UtcNow.AddMonths(-5))
                    .AddNew(new InvoiceItem(0,0){ CreatedDate = DateTime.UtcNow.AddMonths(-5)})
                    .AddNew(new InvoiceItem(0,0){ UpdatedDate = DateTime.UtcNow.AddMonths(-5), CreatedDate = DateTime.UtcNow.AddMonths(-5)}),
                new Invoice(DateTime.UtcNow.AddMonths(-6))
                    .AddNew(new InvoiceItem(0,0){ CreatedDate = DateTime.UtcNow.AddMonths(-7)})
                    .AddNew(new InvoiceItem(0,0){ UpdatedDate = DateTime.UtcNow.AddMonths(-7), CreatedDate = DateTime.UtcNow.AddMonths(-5)}),
                new Invoice(DateTime.UtcNow.AddMonths(-5))
                    .AddNew(new InvoiceItem(0,0){ UpdatedDate = DateTime.UtcNow.AddMonths(-100), CreatedDate = DateTime.UtcNow.AddMonths(-5)})
                    .AddNew(new InvoiceItem(0,0){ CreatedDate = DateTime.UtcNow.AddMonths(-500)}),
            };

            invoices
               .All(_invoiceService.AnyItemChangedSince(baseDate))
               .Should().BeFalse();
        }

        [TestMethod]
        public void Should_return_invoice_with_changes_when_invoice_was_created_since_base_date()
        {
            var invoice = new Invoice(DateTime.UtcNow) { CreatedDate = DateTime.UtcNow };

            _invoiceService.AnyChangeSince(invoice.CreatedDate.AddDays(-100))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(invoice.CreatedDate.AddMilliseconds(-1))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(invoice.CreatedDate.AddMilliseconds(0))(invoice)
                .Should().BeFalse();

            _invoiceService.AnyChangeSince(invoice.CreatedDate.AddMilliseconds(1))(invoice)
                .Should().BeFalse();

        }

        [TestMethod]
        public void Should_return_invoice_with_changes_when_invoice_was_updated_since_base_date()
        {
            var invoice = new Invoice(DateTime.UtcNow)
            {
                CreatedDate = DateTime.UtcNow.AddYears(-100),
                UpdatedDate = DateTime.UtcNow,
            };

            _invoiceService.AnyChangeSince(invoice.UpdatedDate.Value.AddDays(-100))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(invoice.UpdatedDate.Value.AddMilliseconds(-1))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(invoice.UpdatedDate.Value.AddMilliseconds(0))(invoice)
                .Should().BeFalse();

            _invoiceService.AnyChangeSince(invoice.UpdatedDate.Value.AddMilliseconds(1))(invoice)
                .Should().BeFalse();

        }

        [TestMethod]
        public void Should_return_invoice_with_changes_when_invoice_item_was_created_since_base_date()
        {
            var invoice = new Invoice(DateTime.UtcNow)
            {
                CreatedDate = DateTime.UtcNow.AddYears(-100),
                UpdatedDate = DateTime.UtcNow.AddYears(-100),
            };

            var itemDate = DateTime.UtcNow;

            invoice.AddNew(new InvoiceItem(0, 0) { CreatedDate = itemDate });

            _invoiceService.AnyChangeSince(itemDate.AddDays(-100))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(itemDate.AddMilliseconds(-1))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(itemDate.AddMilliseconds(0))(invoice)
                .Should().BeFalse();

            _invoiceService.AnyChangeSince(itemDate.AddMilliseconds(1))(invoice)
                .Should().BeFalse();

        }

        [TestMethod]
        public void Should_return_invoice_with_changes_when_invoice_item_was_updated_since_base_date()
        {
            var itemDate = DateTime.UtcNow;

            var invoice = new Invoice(DateTime.UtcNow)
            {
                CreatedDate = DateTime.UtcNow.AddYears(-100),
                UpdatedDate = DateTime.UtcNow.AddYears(-100),
            };
            invoice.AddNew(new InvoiceItem(0, 0) { UpdatedDate = itemDate });

            _invoiceService.AnyChangeSince(itemDate.AddDays(-100))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(itemDate.AddMilliseconds(-1))(invoice)
                .Should().BeTrue();

            _invoiceService.AnyChangeSince(itemDate)(invoice)
                .Should().BeFalse();

            _invoiceService.AnyChangeSince(itemDate.AddMilliseconds(1))(invoice)
                .Should().BeFalse();

        }

        [TestMethod]
        public void Should_return_true_when_the_invoice_has_the_close_date_after_the_date_informed()
        {
            _invoiceService
                .ClosedInvoiceAfter(DateTime.UtcNow.AddMonths(-1))(new Invoice(DateTime.UtcNow))
                .Should().BeTrue();
        }

        [TestMethod]
        public void Should_return_false_when_the_invoice_not_has_the_close_date_after_the_date_informed()
        {
            _invoiceService
              .ClosedInvoiceAfter(DateTime.UtcNow)(new Invoice(DateTime.UtcNow.AddMonths(-1)))
              .Should().BeFalse();
        }

        [TestMethod]
        public void Should_return_invoice_is_paid_when_invoice_is_paid()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            invoice.WasPaidIn(DateTime.UtcNow);

            _invoiceService.IsPaid(invoice, invoice.PaymentDate.Value).Should().BeTrue();
        }

        [TestMethod]
        public void Should_return_invoice_is_not_paid_when_invoice_was_not_pay()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            _invoiceService.IsPaid(invoice, DateTime.UtcNow).Should().BeFalse();
        }

        [TestMethod]
        public void Should_return_invoice_is_not_paid_based_on_date()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            invoice.WasPaidIn(DateTime.UtcNow);

            _invoiceService.IsPaid(invoice, DateTime.UtcNow.AddDays(-1)).Should().BeFalse();
        }

        [TestMethod]
        [DataRow("10/10/2020", "11/10/2020", 1)]
        [DataRow("10/10/2020", "09/11/2020", 30)]
        [DataRow("10/10/2020", "09/09/2021", 334)]
        [DataRow("10/10/2020", "10/10/2020", 0)]
        [DataRow("11/10/2020", "10/10/2020", -1)]
        [DataRow("10/10/2020", "09/12/2020", 60)]
        public void Should_return_the_correct_days_to_close(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService.DaysToClose(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [Description("Due date is 7 days after close date at this moment")]
        [DataRow("10/10/2020", "11/10/2020", 8)]
        [DataRow("17/10/2020", "10/10/2020", 0)]
        [DataRow("18/10/2020", "10/10/2020", -1)]
        [DataRow("10/10/2020", "09/11/2020", 37)]
        [DataRow("10/10/2020", "09/09/2021", 341)]
        [DataRow("10/10/2020", "10/10/2020", 7)]
        [DataRow("10/10/2020", "09/12/2020", 67)]
        public void Should_return_the_correct_days_to_overdue(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService.DaysToOverdue(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [Description("Due date is 7 days after close date at this moment")]
        [DataRow("18/10/2020", "11/10/2020", 0)]
        [DataRow("18/10/2020", "10/10/2020", 1)]
        [DataRow("16/10/2020", "10/10/2020", -1)]
        [DataRow("09/11/2020", "10/10/2020", 23)]
        [DataRow("09/09/2021", "10/10/2020", 327)]
        [DataRow("10/10/2020", "10/10/2020", -7)]
        [DataRow("09/12/2020", "10/10/2020", 53)]
        public void Should_return_the_correct_overdue_days(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService.OverdueDays(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [Description("Invoice close date is always the last month day at this moment")]
        [DataRow("18/10/2020", "30/11/2020", 14)]
        [DataRow("01/11/2020", "30/11/2020", 0)]
        [DataRow("02/11/2020", "30/11/2020", -1)]
        [DataRow("31/10/2020", "30/11/2020", 1)]
        [DataRow("31/10/2021", "30/11/2020", -364)]
        [DataRow("01/11/2020", "30/11/2021", 365)]
        public void Should_return_the_correct_days_to_open(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService.DaysToOpen(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [DataRow("18/10/2020", "30/11/2020", false)]
        [DataRow("30/11/2020", "30/11/2020", false)]
        [DataRow("01/12/2020", "30/11/2020", true)]
        [DataRow("01/01/2021", "30/11/2020", true)]
        [DataRow("31/12/2019", "30/11/2020", false)]
        public void Should_return_invoice_is_closed_correctly(String baseDateString, String closeDateString, bool expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService
                .IsClosed(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [Description("Due date is 7 days after close date at this moment")]
        [DataRow("18/10/2020", "30/11/2020", false)]
        [DataRow("07/12/2020", "30/11/2020", false)]
        [DataRow("08/12/2020", "30/11/2020", true)]
        [DataRow("01/01/2021", "30/11/2020", true)]
        [DataRow("31/12/2019", "30/11/2020", false)]
        public void Should_return_invoice_is_overdue_correctly(String baseDateString, String closeDateString, bool expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService
                .IsOverdue(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [Description("Close date day is always the last day in th month at this moment")]
        [DataRow("01/05/2022", "31/05/2022", true)]
        [DataRow("31/05/2022", "31/05/2022", true)]
        [DataRow("30/05/2022", "31/05/2022", true)]
        [DataRow("02/05/2022", "31/05/2022", true)]
        [DataRow("31/05/2022", "31/05/2022", true)]
        [DataRow("01/06/2022", "31/05/2022", false)]
        [DataRow("02/05/2023", "31/05/2022", false)]
        [DataRow("02/07/2022", "31/05/2022", false)]
        public void Should_return_invoice_is_opened_correctly(String baseDateString, String closeDateString, bool expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService
                .IsOpened(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        public void Should_return_invoice_status_when_invoice_is_paid()
        {
            var invoice = new Invoice(DateTime.UtcNow)
                .WasPaidIn(DateTime.UtcNow.AddDays(-1));

            _invoiceService
                .Status(invoice, DateTime.UtcNow)
                .Should().Be(InvoiceStatus.Paid);

            _invoiceService
               .Status(invoice, invoice.PaymentDate.Value)
               .Should().Be(InvoiceStatus.Paid);

            _invoiceService
               .Status(invoice, invoice.PaymentDate.Value.AddMilliseconds(-1))
               .Should().NotBe(InvoiceStatus.Paid, because: "The base date is before the payment date");
        }

        [TestMethod]
        public void Should_return_invoice_status_when_invoice_is_not_paid()
        {
            var invoice = new Invoice(DateTime.UtcNow.AddYears(-100));

            _invoiceService
                .Status(invoice, DateTime.UtcNow)
                .Should().NotBe(InvoiceStatus.Paid, because: "Invoice was never paid");
        }

        [TestMethod]
        public void Should_return_invoice_status_when_invoice_is_overdue()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            _invoiceService
                .Status(invoice, invoice.DueDate)
                .Should().NotBe(InvoiceStatus.Overdue);

            _invoiceService
               .Status(invoice, invoice.DueDate.AddDays(1))
               .Should().Be(InvoiceStatus.Overdue);

            _invoiceService
               .Status(invoice, invoice.DueDate.AddDays(-1))
               .Should().NotBe(InvoiceStatus.Overdue);
        }

        [TestMethod]
        public void Should_return_invoice_status_when_invoice_is_closed()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            _invoiceService
                .Status(invoice, invoice.CloseDate)
                .Should().NotBe(InvoiceStatus.Closed);

            _invoiceService
               .Status(invoice, invoice.CloseDate.AddDays(1))
               .Should().Be(InvoiceStatus.Closed);

            _invoiceService
               .Status(invoice, invoice.CloseDate.AddDays(-1))
               .Should().NotBe(InvoiceStatus.Closed);
        }

        [TestMethod]
        public void Should_return_invoice_status_when_invoice_is_opened()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            _invoiceService
                .Status(invoice, invoice.CloseDate)
                .Should().Be(InvoiceStatus.Open);

            _invoiceService
               .Status(invoice, invoice.CloseDate.AddDays(1))
               .Should().NotBe(InvoiceStatus.Open);

            _invoiceService
               .Status(invoice, invoice.CloseDate.AddDays(-1))
               .Should().Be(InvoiceStatus.Open);
        }

        [TestMethod]
        public void Should_return_invoice_status_when_invoice_is_future()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            //The negative value below is in the base date, so,
            //it is like I'm consulting the current invoice in the past
            _invoiceService
                .Status(invoice, invoice.CloseDate.AddMonths(-1))
                .Should().Be(InvoiceStatus.Future);

            _invoiceService
               .Status(invoice, invoice.CloseDate.AddMonths(-10))
               .Should().Be(InvoiceStatus.Future);

            _invoiceService
               .Status(invoice, invoice.CloseDate)
               .Should().NotBe(InvoiceStatus.Future);
        }

        [TestMethod]
        [DataRow("31/05/2022", "31/05/2022", 0)]
        [DataRow("01/06/2022", "31/05/2022", 0)]
        [DataRow("02/05/2023", "31/05/2022", 0)]
        [DataRow("02/07/2022", "31/05/2022", 0)]
        public void Should_return_invoice_days_remaining_when_paid_correctly(String baseDateString, String paymentDateString, int expected)
        {
            var paymentDate = DateTime.ParseExact(paymentDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(paymentDate)
                .WasPaidIn(paymentDate);

            _invoiceService
                .DaysRemainingToNextStage(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [Description("Due date is 7 days after close date at this moment")]
        [DataRow("01/06/2022", "31/05/2022", 6)]
        [DataRow("07/06/2022", "31/05/2022", 0)]
        [DataRow("06/06/2022", "31/05/2022", 1)]
        public void Should_return_invoice_days_remaining_when_closed_correctly(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService
                .DaysRemainingToNextStage(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [DataRow("30/05/2022", "31/05/2022", 1)]
        [DataRow("31/05/2022", "31/05/2022", 0)]
        [DataRow("01/04/2022", "31/05/2022", 30)]
        public void Should_return_invoice_days_remaining_when_open_correctly(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService
                .DaysRemainingToNextStage(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [Description("Due date is 7 days after close date at this moment")]
        [DataRow("08/06/2022", "31/05/2022", 1)]
        [DataRow("02/07/2022", "31/05/2022", 25)]
        [DataRow("03/03/2023", "31/05/2022", 269)]
        public void Should_return_invoice_days_remaining_when_overdue_correctly(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService
                .DaysRemainingToNextStage(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        [DataRow("29/04/2022", "31/05/2022", 2)]
        [DataRow("30/04/2022", "31/05/2022", 1)]
        [DataRow("31/12/2021", "31/05/2022", 121)]
        public void Should_return_invoice_days_remaining_to_open_correctly(String baseDateString, String closeDateString, int expected)
        {
            var closeDate = DateTime.ParseExact(closeDateString, _dateFormat, _ptbr);
            var baseDate = DateTime.ParseExact(baseDateString, _dateFormat, _ptbr);

            var invoice = new Invoice(closeDate);

            _invoiceService
                .DaysRemainingToNextStage(invoice, baseDate)
                .Should().Be(expected);
        }

        [TestMethod]
        public void Should_return_status_changed_when_the_invoice_status_change_based_on_the_date()
        {
            var invoice = new Invoice(DateTime.UtcNow);

            _invoiceService.StatusChanged(invoice.CloseDate, invoice.DueDate)(invoice)
                .Should().BeTrue();

            _invoiceService.StatusChanged(invoice.CloseDate, invoice.CloseDate)(invoice)
                .Should().BeFalse();

            _invoiceService.StatusChanged(invoice.DueDate, invoice.DueDate)(invoice)
                .Should().BeFalse();
        }

    }
}
