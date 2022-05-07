namespace Expenses.DTO.Expenses
{
    public record ExpenseItemDTO
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public decimal Cost { get; set; }

        public int Amount { get; set; }

        public Guid ExpenseId { get; set; }
    }
}
