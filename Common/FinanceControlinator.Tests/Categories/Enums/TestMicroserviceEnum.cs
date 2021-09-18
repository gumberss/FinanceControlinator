using System.ComponentModel;

namespace FinanceControlinator.Tests.Categories.Enums
{
    public enum TestMicroserviceEnum
    {
        [Description("Invoices")]
        Invoices,

        [Description("Expenses")]
        Expenses
    }
}
