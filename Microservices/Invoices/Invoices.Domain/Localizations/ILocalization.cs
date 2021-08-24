using System;

namespace Invoices.Domain.Localizations
{
    public interface ILocalization
    {
        String DATE_INCORRECT { get; }
        string PAYMENT_ALREADY_EXISTENT { get; }
    }
}
