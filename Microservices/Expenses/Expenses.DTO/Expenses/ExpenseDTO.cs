namespace Expenses.DTO.Expenses
{
    public record ExpenseDTO
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime PurchaseDate { get; set; }

        //TODO: use only as ExpenseType, but it is not possible while the enum ExpenseType from models are using as parameter in API
        public ExpenseTypeDTO Type { get; set; }

        public int InstallmentsCount { get; set; }

        public String Location { get; set; }

        public String Observation { get; set; }

        public decimal TotalCost { get; set; }

        public Guid UserId { get; set; }

        public List<ExpenseItemDTO> Items { get; set; }
    }
}
