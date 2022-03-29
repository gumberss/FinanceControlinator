## Why use event driven instead of message driven to generate invoices

### Context
- Message Driven (1):
![image](https://user-images.githubusercontent.com/38296002/134834584-1ff7dafb-274f-44ac-80ea-6d59b3acfedc.png)
- Event Driven (2):
![image](https://user-images.githubusercontent.com/38296002/134834624-db700cd9-9f0e-4efc-ac7d-05a7a10c3c6a.png)

- If we consider to change Event Driven for Message Driven (2) we will need to know the invoices microsservices bussines rules on piggy banks microservice
![image](https://user-images.githubusercontent.com/38296002/134834777-172f83d0-8108-491a-a95f-8b2073c2f682.png)
- So, we decide to use event driven just because it.
- It is the best way to go? Probably not, but it is the better way I can think.
