using System;

namespace Accounts.Domain.Localizations
{
    public interface ILocalization
    {
        string PAYMENT_ITEM_NOT_CLOSED { get; }
        string PAYMENT_ALREADY_IN_PROCESS { get; }
        string ITEM_ALREADY_WAS_PAID { get; }
        string ITEM_NOT_FOUND { get; }
    }
}
