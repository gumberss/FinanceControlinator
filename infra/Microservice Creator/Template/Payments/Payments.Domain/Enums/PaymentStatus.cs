namespace Payments.Domain.Enums
{
    public enum PaymentStatus
    {
        Opened = 0,
        PaymentRequested = 10,
        Paid = 20,
        PaymentRejected = 30,
    }
}
