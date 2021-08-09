
# General

## Architecture

### Protecting the domain code
- First of all, the creator of this project, does not like infrastructure code inside domain code
- Other important thing is, the creator of this project consider the application layer as domain layer, because it has the flow of the business rules. 
- Focused on protect the business rules, AppService, Services and Entities are isolated from infrastructure code, like data persistence, message broken, message processor...
- Of course, some things we need to "accept", like logs, database repositories, dbContext, documentStore, and so on, BUT, only the interfaces that they inherit, making easy to mock objects inside tests and replace for other framework that make the same thing (if needed) for some reason.
- Because we need somewhere to join infrastructure code and domain code, the Application Layer has this responsibility, but working only with infrastructure interfaces
- Do not use repository inside domain services and domain entities, use it only inside application layer, referencing the interface, of course

### Events
- Only outside of domain/application layers you can publish integration events
	- The best ways to do it is publishing events from Controllers and from integration handlers
	- All created events must be named with what they did , in the past
	- Every handler must react on happened events
	- Never publish a event that specify the next process, if you do this, you will couple two microservices

## Frameworks
 ### MediatR
 - From API, you can send:
	- Commands: To make some business rule synchronized with the user (that users must be notified just after the process was ended)
	- Queries: To retrieve something from the database
- From Handlers:
	- Commands: With only the necessary data to use in this proccess (filtering integration event data): To make some business rule
	- Queries: It does not make any sense to me....