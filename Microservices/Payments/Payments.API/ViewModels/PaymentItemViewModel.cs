using System;

namespace Payments.API.ViewModels
{
    public class PaymentItemViewModel
    {
        public String Name { get; init; }

        public String Description { get; init; }

        public decimal Cost { get; set; }

        public int Amount { get; init; }

        public Guid PaymentId { get; set; }

    }
}
