using System;

namespace FinanceControlinator.Events.Payments
{
    public class PaymentRejectedEvent
    {
        public Guid Id { get; set; }

        public String Reason { get; set; }
    }
}
