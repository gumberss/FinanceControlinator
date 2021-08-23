using FinanceControlinator.Events.Payments.DTOs;

namespace FinanceControlinator.Events.Payments
{
    public class PaymentPerformedEvent
    {
        public PaymentDTO Payment { get; set; }
    }
}
