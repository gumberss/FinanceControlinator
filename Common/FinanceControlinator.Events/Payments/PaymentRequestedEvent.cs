using FinanceControlinator.Events.Payments.DTOs;

namespace FinanceControlinator.Events.Payments
{
    public class PaymentRequestedEvent
    {
        public PaymentDTO Payment { get; set; }
    }
}
