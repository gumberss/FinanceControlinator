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
    public class InvoiceServiceTest
    {
        private static InvoiceService _invoiceService;
        private static DateTime _purchaseDay;
        private Expense _expense;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _invoiceService = new InvoiceService();
            _purchaseDay = new DateTime(2021, 08, 05);
        }

        [TestInitialize]
        public void Init()
        {
            _expense = new Expense()
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
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
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
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
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
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
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
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Should_create_a_new_invoice_when_the_month_invoice_already_exist_but_of_the_other_year()
        {
            var existentInvoiceDate = new DateTime(2004, 08, 05);

            var existentInvoice = new Invoice(closeDate: existentInvoiceDate)
            {
                Id = Guid.NewGuid().ToString()
            };
            existentInvoice.AddNew(new InvoiceItem(1, 100));

            var existentInvoices = new List<Invoice>() { existentInvoice };

            var currentInvoiceDate = _purchaseDay;

            var invoices = _invoiceService.RegisterExpense(_expense, existentInvoices, currentInvoiceDate);

            invoices.Should().HaveCount(2);

            invoices.Except(existentInvoices).Should().HaveCount(2);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
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
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        [DataRow(2021, 9, 1, 2021, 9, 30)]
        [DataRow(2021, 9, 15, 2021, 9, 30)]
        [DataRow(2021, 9, 30, 2021, 9, 30)]
        [DataRow(2021, 10, 30, 2021, 10, 31)]
        public void Should_find_the_correct_close_date_by_a_base_date(
            int baseYear, int baseMonth, int baseDay
          , int closeYear, int closeMonth, int closeDay)
        {
            var baseDate = new DateTime(baseYear, baseMonth, baseDay);

            _expense.InstallmentsCount = 3;
            _expense.TotalCost = 100;

            var invoiceCloseDate = _invoiceService.GetInvoiceCloseDateBy(baseDate);

            invoiceCloseDate.Year.Should().Be(closeYear);
            invoiceCloseDate.Month.Should().Be(closeMonth);
            invoiceCloseDate.Day.Should().Be(closeDay);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
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
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_maior_que_o_dia_da_data_inicial_de_meses_diferente()
        {
            var startDate = new DateTime(2021, 09, 10);
            var endDate = new DateTime(2021, 12, 20);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(3);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_maior_que_o_dia_da_data_inicial_do_mesmo_mes()
        {
            var startDate = new DateTime(2021, 09, 20);
            var endDate = new DateTime(2021, 09, 30);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(1);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_menor_que_o_dia_da_data_inicial_de_meses_diferente()
        {
            var startDate = new DateTime(2021, 09, 20);
            var endDate = new DateTime(2021, 12, 10);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(3);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_o_dia_da_data_final_foi_menor_que_o_dia_da_data_inicial_do_mesmo_mes()
        {
            var startDate = new DateTime(2021, 09, 10);
            var endDate = new DateTime(2021, 09, 20);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(0);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        [DataRow("10/09/2021", "09/09/2021")]
        [DataRow("10/10/2021", "11/09/2021")]
        [DataRow("10/10/2021", "11/11/2020")]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_a_data_final_for_menor_que_a_data_inicial
            (String startDateString, String endDateString)
        {
            string dateFormat = "dd/MM/yyyy";

            CultureInfo ptbr = new CultureInfo("pt-BR");

            var startDate = DateTime.ParseExact(startDateString, dateFormat, ptbr);
            var endDate = DateTime.ParseExact(endDateString, dateFormat, ptbr);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(0);
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Invoices, TestFeatureEnum.InvoiceGeneration)]
        [DataRow("10/10/2020", "11/11/2021")]
        public void Deveria_encontrar_a_quantidade_de_parcelas_quando_a_data_final_for_menor_que_a_data_inicial22222222
          (String startDateString, String endDateString)
        {
            string dateFormat = "dd/MM/yyyy";

            CultureInfo ptbr = new CultureInfo("pt-BR");

            var startDate = DateTime.ParseExact(startDateString, dateFormat, ptbr);
            var endDate = DateTime.ParseExact(endDateString, dateFormat, ptbr);

            var installments = _invoiceService.GetInvoiceInstallmentsByDateRange(startDate, endDate);

            installments.Should().Be(13);
        }
    }
}
