using System.ComponentModel;

namespace FinanceControlinator.Tests.Categories.Enums
{
    public enum TestUserJourneyEnum
    {
        [Description("Recording Expenses")]
        RecordingExpenses,

        [Description("Invoice Payment")]
        InvoicePayment,

        [Description("Recording Piggy Banks")]
        RecordingPiggyBanks,

        [Description("Overview")]
        Overview,

        [Description("Expense General")]
        ExpenseGeneral,
    }
}
