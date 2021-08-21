using System;

namespace FinanceControlinator.Events.Accounts.DTOs
{
    public class AccountChangeDTO
    {
        public Guid Id { get; set; }

        public decimal AmountChanged { get; set; }

        public decimal OldAmount { get; set; }

        public decimal NewAmount { get; set; }

        public Guid PaymentId { get; set; }
    }
}
