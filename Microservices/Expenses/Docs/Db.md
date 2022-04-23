## Migrations
### Create a new migration
	- dotnet ef migrations add Migration_name --project Microservices/Expenses/Expenses.Data --startup-project Microservices/Expenses/Expenses.API --context ExpenseDbContext
### Apply a migration
	- dotnet ef database update --project Microservices/Expenses/Expenses.Data --startup-project Microservices/Expenses/Expenses.API --context ExpenseDbContext
