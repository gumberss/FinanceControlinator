using System;


namespace FinanceControlinator.Events.Payments
{
    public class RegisterItemToPayEvent 
    {
        public Guid Id { get; set; }

        public String Title { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CloseDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public String DetailsPath { get; set; }
    }
}
