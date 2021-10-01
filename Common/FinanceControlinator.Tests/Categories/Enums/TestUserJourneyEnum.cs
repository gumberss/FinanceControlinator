using System.ComponentModel;

namespace FinanceControlinator.Tests.Categories.Enums
{
    public enum TestUserJourneyEnum
    {
        [Description("Recording Expenses")]
        RecordingExpenses,

        [Description("Invoice Payment")]
        InvoicePayment,
    }
}
