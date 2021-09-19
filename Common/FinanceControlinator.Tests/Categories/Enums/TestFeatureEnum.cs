using System.ComponentModel;

namespace FinanceControlinator.Tests.Categories.Enums
{
    public enum TestFeatureEnum
    {
        [Description("Invoice Generation")]
        InvoiceGeneration,

        [Description("Expense Generation")]
        ExpenseGeneration,

        [Description("Expense Update")]
        ExpenseUpdate
    }
}
