using System;
using System.Collections.Generic;

namespace FinanceControlinator.Events.Payments.DTOs
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public String Description { get; set; }

        public DateTime? Date { get; set; }

        public int Status { get; set; }

        public String ItemId { get; set; }

        public decimal TotalAmount { get; set; }

        public List<PaymentMethodDTO> PaymentMethods { get; set; }
    }
}
