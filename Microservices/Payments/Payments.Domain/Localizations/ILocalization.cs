using System;

namespace Payments.Domain.Localizations
{
    public interface ILocalization
    {
        string PAYMENT_ITEM_NOT_CLOSED { get; }
        string PAYMENT_ALREADY_IN_PROCESS { get; }
        string ITEM_ALREADY_WAS_PAID { get; }
        string ITEM_NOT_FOUND { get; }
        string PAYMENT_NOT_FOUND { get; }
        string PAYMENT_IS_NOT_WAITING_FOR_CONFIRMATION { get; }
    }
}
