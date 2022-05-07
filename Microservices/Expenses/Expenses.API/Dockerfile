#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Microservices/Expenses/Expenses.API/Expenses.API.csproj", "Microservices/Expenses/Expenses.API/"]
COPY ["Microservices/Expenses/Expenses.Domain/Expenses.Domain.csproj", "Microservices/Expenses/Expenses.Domain/"]
COPY ["Common/FinanceControlinator.Common/FinanceControlinator.Common.csproj", "FinanceControlinator.Common/"]
COPY ["Common/FinanceControlinator.Events/FinanceControlinator.Events.csproj", "FinanceControlinator.Events/"]
COPY ["Microservices/Expenses/Expenses.Data/Expenses.Data.csproj", "Microservices/Expenses/Expenses.Data/"]
COPY ["Microservices/Expenses/Expenses.DTO/Expenses.DTO.csproj", "Microservices/Expenses/Expenses.DTO/"]
COPY ["Microservices/Expenses/Expenses.Handler/Expenses.Handler.csproj", "Microservices/Expenses/Expenses.Handler/"]
COPY ["Microservices/Expenses/Expenses.Application/Expenses.Application.csproj", "Microservices/Expenses/Expenses.Application/"]
RUN dotnet restore "Microservices/Expenses/Expenses.API/Expenses.API.csproj"
COPY . .
WORKDIR "Microservices/Expenses/Expenses.API"
RUN dotnet build "Expenses.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Expenses.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Expenses.API.dll"]